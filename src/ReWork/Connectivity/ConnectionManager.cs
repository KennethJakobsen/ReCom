using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReWork.Config.Roles;
using ReWork.Handlers;
using ReWork.SystemMessages;

#pragma warning disable 4014

namespace ReWork.Connectivity
{
    internal class ConnectionManager : IConnectionManager, INotifyTermination
    {
        private readonly ConcurrentDictionary<string, Connection> _connections = new ConcurrentDictionary<string, Connection>();
        private readonly IConnectionFactory _factory;
        private readonly IHandlerDispatcher _dispatcher;
        private readonly ITransportManager _transportManager;

        public ConnectionManager(IConnectionFactory factory, IHandlerDispatcher dispatcher, ITransportManager transportManager)
        {
            _factory = factory;
            _dispatcher = dispatcher;
            _transportManager = transportManager;
        }
        public async Task StartListening(ReWorkServerRole role)
        {
            var cts = new CancellationTokenSource();
            try
            {
                await _transportManager.StartListeningAsync(role, cts.Token);
                
                await AcceptClientsAsync(cts.Token);
            }
            finally
            {
                cts.Cancel();
                await _transportManager.StopListeningAsync();
            }
        }
        private async Task AcceptClientsAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                CheckClients(ct);
                var tConnection = await _transportManager.AcceptClientAsync().ConfigureAwait(false);
                var connection = _factory.Create(Guid.NewGuid().ToString(), tConnection, this);
                _connections.TryAdd(connection.ClientId, connection);

                //once again, just fire and forget, and use the CancellationToken
                //to signal to the "forgotten" async invocation.
                connection.ProcessCommandsAsync(ct);
            }

        }
        public async Task<Connection> Connect(ReWorkClientRole role)
        {

            var cts = new CancellationTokenSource();
            var tConnection = await _transportManager.ConnectAsync(role, cts.Token);
            var connection = _factory.Create(Guid.Empty.ToString(),tConnection, this);

            //once again, just fire and forget, and use the CancellationToken
            //to signal to the "forgotten" async invocation.
            connection.Authorize();
            connection.ProcessCommandsAsync(cts.Token);
            await connection.Send(new InitiateHandshakeMessage());
            return connection;
        }

        public IEnumerable<Connection> GetConnections(IEnumerable<string> ids)
        {
            return ids.Where(_connections.ContainsKey).Select(x => _connections[x]);
        }

        private async Task CheckClients(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                Connection key = null;
                foreach (var connection in _connections)
                {
                    if (!connection.Value.IsConnected())
                        key = connection.Value;
                }

                if (key != null)
                {
                    _connections.TryRemove(key.ClientId, out var conn);
                    if(conn != null)
                        await _dispatcher.Execute(new DisconnectMessage(), conn);
                }

                await Task.Delay(200, ct);
            }
        }
        public void Terminate(string connectionName)
        {
            _connections.TryRemove(connectionName, out var conn);
            conn.Dispose();
        }
    }
}
