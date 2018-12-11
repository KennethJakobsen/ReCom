using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ReWork.Bson;
using ReWork.Connectivity.Delivery;
using ReWork.Handlers;
using ReWork.Protocol;
using ReWork.SystemMessages;
using ReWork.SystemMessages.Transport;

namespace ReWork.Connectivity
{
    public class Connection : IDisposable
    {

        private readonly TcpClient _client;
        private readonly IProtocol _protocol;
        private readonly ICommandConverter _commandConverter;
        private readonly IHandlerDispatcher _dispatcher;
        private readonly Dictionary<Guid, DeliveryKeeper> _devliverables = new Dictionary<Guid, DeliveryKeeper>();

        internal Connection(TcpClient client, Guid clientId, IProtocol protocol, ICommandConverter commandConverter, IHandlerDispatcher dispatcher)
        {
            _client = client;
            _protocol = protocol;
            _commandConverter = commandConverter;
            _dispatcher = dispatcher;
            ClientId = clientId;
            Stream = _client.GetStream();
        }

        public Guid ClientId { get; private set; }
        internal NetworkStream Stream { get; }

        public async Task Send(object command, bool requireFeedback = false, bool requireHandled = false)
        {
            
            try
            {
                var transport = new TransportMessage()
                {
                    MessageId = Guid.NewGuid(),
                    Payload = command,
                    RequiresHandledFeedback = requireHandled,
                    RequiresReceivedFeedback = requireFeedback
                };

                if (requireFeedback || requireHandled)
                    _devliverables.Add(transport.MessageId, new DeliveryKeeper(command, requireFeedback, requireHandled));

                var commandBytes = _commandConverter.Serialize(transport);
                await _protocol.WriteCommandToStream(commandBytes, Stream);
            }
            catch (IOException)
            {
                Dispose();
            }

        }

        internal void UpdateId(Guid id)
        {
            ClientId = id;
        }
        public void Dispose()
        {
            _client.Dispose();
        }

        internal async Task ProcessCommandsAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var timeoutTask = Task.Delay(TimeSpan.FromHours(2), ct);
                    var readTask = _protocol.ReadCommandFromStream(Stream, ct);
                    var completedTask = await Task.WhenAny(timeoutTask, readTask)
                        .ConfigureAwait(false);
                    TransportMessage command;
                    if (completedTask == timeoutTask)
                    {
                        command = new TransportMessage() { Payload = new TimeoutMessage("Connection timed out")};
                        break;
                    }

                    //now we know that the amountTask is complete so
                    //we can ask for its Result without blocking
                    var commandBytes = readTask.Result;
                    command = _commandConverter.Deserialize(commandBytes);

                    if (command != null)
                    {
                        await SendReceiveConfirmation(command);
                        await _dispatcher.Execute(command.Payload, this);
                        await SendHandledConfirmation(command);
                    }

                    Thread.Sleep(5);
                }
                catch (IOException)
                {
                    break;
                }
            }
            Dispose();
        }

        internal object MarkAsHandled(Guid id)
        {
            var obj = _devliverables[id];
            obj.Handle();
            if (obj.IsDone)
                _devliverables.Remove(id);
            return obj;
        }

        internal object MarkAsReceived(Guid id)
        {
            var obj = _devliverables[id];
            obj.Receive();
            if (obj.IsDone)
                _devliverables.Remove(id);
            return obj;
        }

        private async Task SendReceiveConfirmation(TransportMessage transport)
        {
            if (transport.RequiresReceivedFeedback)
                await Send(new ReceivedMessage() {ReceivedMessageId = transport.MessageId});
        }

        private async Task SendHandledConfirmation(TransportMessage transport)
        {
            if (transport.RequiresHandledFeedback)
                await Send(new HandledMessage() { HandledMessageId = transport.MessageId });
        }


    }
}
