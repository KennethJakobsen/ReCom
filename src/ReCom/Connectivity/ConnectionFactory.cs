using ReCom.Handlers;

namespace ReCom.Connectivity
{
    internal class ConnectionFactory : IConnectionFactory
    {
        private readonly IHandlerDispatcher _dispatcher;

        public ConnectionFactory(  IHandlerDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        public Connection Create( string clientId, ITransportConnection connection, INotifyTermination terminator)
        {
            return new Connection(clientId, connection, _dispatcher, terminator);
        }
    }
}
