using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using ReWork.Config.Roles;
using ReWork.Handlers;
using ReWork.SystemMessages;

#pragma warning disable 4014

namespace ReWork.Connectivity
{
    public class ClientConnectionManager : IClientConnectionManager
    {
        private readonly ConcurrentDictionary<string, Connection> _connections = new ConcurrentDictionary<string, Connection>();
        private readonly IConnectionFactory _factory;
        private readonly IHandlerDispatcher _dispatcher;
        private readonly IClientTransportManager _transportManager;

        public ClientConnectionManager(IConnectionFactory factory, IHandlerDispatcher dispatcher, IClientTransportManager transportManager)
        {
            _factory = factory;
            _dispatcher = dispatcher;
            _transportManager = transportManager;
        }
        public async Task<Connection> Connect(ReWorkClientRole role)
        {

            var cts = new CancellationTokenSource();
            var tConnection = await _transportManager.ConnectAsync(role, cts.Token);
            var connection = _factory.Create(Guid.Empty.ToString(), tConnection);

            //once again, just fire and forget, and use the CancellationToken
            //to signal to the "forgotten" async invocation.
            connection.Authorize();
            connection.ProcessCommandsAsync(cts.Token);
            await connection.Send(new InitiateHandshakeMessage());
            return connection;
        }
    }
}
