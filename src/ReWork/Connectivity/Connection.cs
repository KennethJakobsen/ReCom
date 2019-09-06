using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ReWork.Connectivity.Delivery;
using ReWork.Handlers;
using ReWork.SystemMessages;
using ReWork.SystemMessages.Transport;

namespace ReWork.Connectivity
{
    public class Connection : IDisposable
    {
        private readonly ITransportConnection _transport;
        private readonly IHandlerDispatcher _dispatcher;
        private readonly Dictionary<Guid, DeliveryKeeper> _devliverables = new Dictionary<Guid, DeliveryKeeper>();
        private readonly INotifyTermination _connectionTerminator;

        internal Connection(string clientId, ITransportConnection transport, IHandlerDispatcher dispatcher, INotifyTermination connectionTerminator = null)
        {
            _transport = transport;
            _dispatcher = dispatcher;
            ClientId = clientId;
            _connectionTerminator = connectionTerminator;
            AuthorizedForReceive = false;
        }

        ~Connection()
        {
            Dispose();
        }
        public string ClientId { get; private set; }
        public bool AuthorizedForReceive { get; protected set; }

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
                var transport = new CommandTransportMessage()
                {
                    Payload = command,
                    RequiresHandledFeedback = requireHandled,
                    RequiresReceivedFeedback = requireFeedback
                };

                if (requireFeedback || requireHandled)
                    _devliverables.Add(transport.MessageId, new DeliveryKeeper(command, requireFeedback, requireHandled));

                await _transport.WriteCommandAsync(transport);
            }
            catch (IOException)
            {
                Dispose();
            }

        }

        public async Task Request(object command)
        {
            try
            {
                var transport = new RequestTransportMessage()
                {
                    Payload = command,
                    
                };
                await _transport.WriteCommandAsync(transport);
            }
            catch (IOException)
            {
                Dispose();
            }
        }

        public bool IsConnected()
        {
            return _transport.IsConnectedAsync().Result;
        }
        internal void UpdateId(string id)
        {
           ClientId = id;
        }
        public void Dispose()
        {
            _transport.Dispose();
        }

        internal async Task ProcessCommandsAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var command = await _transport.ReadCommandAsync(ct);

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
            await _transport.DisconnectAsync();
            _connectionTerminator?.Terminate(ClientId);
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

        private async Task SendReceiveConfirmation(ITransportMessages transport)
        {
            if (!(transport is CommandTransportMessage msgTransport))
                throw new InvalidCastException();

            if (msgTransport.RequiresReceivedFeedback)
                await Send(new ReceivedMessage() {ReceivedMessageId = transport.MessageId});
        }

        private async Task SendHandledConfirmation(ITransportMessages transport)
        {
            if (!(transport is CommandTransportMessage msgTransport))
                throw new InvalidCastException();

            if (msgTransport.RequiresHandledFeedback)
                await Send(new HandledMessage() { HandledMessageId = transport.MessageId });
        }


    }
}
