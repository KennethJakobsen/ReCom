using System;
using System.Net.Sockets;
using ReWork.Bson;
using ReWork.Handlers;
using ReWork.Protocol;

namespace ReWork.Connectivity
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IProtocol _protocol;
        private readonly ICommandConverter _commandConverter;
        private readonly IHandlerDispatcher _dispatcher;

        public ConnectionFactory(IProtocol protocol, ICommandConverter commandConverter, IHandlerDispatcher dispatcher)
        {
            _protocol = protocol;
            _commandConverter = commandConverter;
            _dispatcher = dispatcher;
        }
        public Connection Create(TcpClient client, string clientId, INotifyTermination terminator)
        {
            return new Connection(client, clientId, _protocol, _commandConverter, _dispatcher, terminator);
        }
    }
}
