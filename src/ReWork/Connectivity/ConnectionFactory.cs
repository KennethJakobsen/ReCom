using System;
using System.Net.Sockets;
using ReWork.Bson;
using ReWork.Protocol;

namespace ReWork.Connectivity
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IProtocol _protocol;
        private readonly ICommandConverter _commandConverter;

        public ConnectionFactory(IProtocol protocol, ICommandConverter commandConverter)
        {
            _protocol = protocol;
            _commandConverter = commandConverter;
        }
        public Connection Create(TcpClient client, Guid clientId)
        {
            return new Connection(client, clientId, _protocol, _commandConverter);
        }
    }
}
