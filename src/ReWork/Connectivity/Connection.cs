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
        private readonly INotifyTermination _connectionTerminator;

        internal Connection(TcpClient client, string clientId, IProtocol protocol, ICommandConverter commandConverter, IHandlerDispatcher dispatcher, INotifyTermination connectionTerminator)
        {
            _client = client;
            _protocol = protocol;
            _commandConverter = commandConverter;
            _dispatcher = dispatcher;
            ClientId = clientId;
            Stream = _client.GetStream();
            _connectionTerminator = connectionTerminator;
            AuthorizedForReceive = false;
        }

        public string ClientId { get; private set; }
        public bool AuthorizedForReceive { get; protected set; }
        internal NetworkStream Stream { get; }

        public virtual void Authorize()
        {
            AuthorizedForReceive = true;
        }

        public virtual void DenyAuthorization()
        {
            AuthorizedForReceive = false;
        }

        public async Task Send(object command, bool requireFeedback = false, bool requireHandled = false)
        {
            
            try
            {
                var transport = new TransportMessage()
                {
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

        internal void UpdateId(string id)
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
                        if (!AuthorizedForReceive && !(command.Payload is InitiateHandshakeMessage))
                        {
                            await Send(new NotAuthorizedMessage("You are not authorized to send messages"));
                            continue;
                        }

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


        public async Task Terminate(string reason = null)
        {
            await Send(new ConnectionTerminatingMessage(reason));
            _client.Close();
            _connectionTerminator.Terminate(ClientId);
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

    public interface INotifyTermination
    {
        void Terminate(string connectionName);
    }
}
