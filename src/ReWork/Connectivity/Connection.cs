using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using ReWork.Bson;
using ReWork.Protocol;

namespace ReWork.Connectivity
{
    public class Connection : IDisposable
    {

        private readonly TcpClient _client;
        private readonly IProtocol _protocol;
        private readonly ICommandConverter _commandConverter;
        private readonly Dictionary<Guid, object> _undeliveredCommands = new Dictionary<Guid, object>();

        internal Connection(TcpClient client, Guid clientId, IProtocol protocol, ICommandConverter commandConverter)
        {
            _client = client;
            _protocol = protocol;
            _commandConverter = commandConverter;
            ClientId = clientId;
            Stream = _client.GetStream();
        }

        public Guid ClientId { get; private set; }
        internal NetworkStream Stream { get; }

        public async Task Send(object command)
        {
            try
            {
                var commandBytes = _commandConverter.Serialize(command);
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
    }
}
