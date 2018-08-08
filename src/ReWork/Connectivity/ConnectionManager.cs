using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ReWork.Bson;
using ReWork.Config.Roles;
using ReWork.Handlers;
using ReWork.Protocol;
using ReWork.SystemMessages;

#pragma warning disable 4014

namespace ReWork.Connectivity
{
    internal class ConnectionManager : IConnectionManager
    {
        private readonly Dictionary<Guid, Connection> _connections = new Dictionary<Guid, Connection>();
        private readonly IConnectionFactory _factory;
        private readonly IHandlerDispatcher _dispatcher;
        private readonly IProtocol _protocol;
        private readonly ICommandConverter _commandConverter;

        public ConnectionManager(IConnectionFactory factory, IHandlerDispatcher dispatcher, IProtocol protocol, ICommandConverter commandConverter)
        {
            _factory = factory;
            _dispatcher = dispatcher;
            _protocol = protocol;
            _commandConverter = commandConverter;
        }
        public async Task StartListening(ReWorkServerRole role)
        {
            var cts = new CancellationTokenSource();
            var listener = new TcpListener(role.IpAddress, role.Port);
            try
            {
                listener.Start();
                await AcceptClientsAsync(listener, cts.Token);
            }
            finally
            {
                cts.Cancel();
                listener.Stop();
            }
        }
        private async Task AcceptClientsAsync(TcpListener listener, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync()
                    .ConfigureAwait(false);
                var connection = _factory.Create(client, Guid.NewGuid());
                _connections.Add(connection.ClientId, connection);

                //once again, just fire and forget, and use the CancellationToken
                //to signal to the "forgotten" async invocation.
                connection.ProcessCommandsAsync(ct);
            }

        }
        public async Task<Connection> Connect(ReWorkClientRole role)
        {
            var cts = new CancellationTokenSource();
            var client = new TcpClient(role.Host, role.Port);
            var connection = _factory.Create(client, Guid.Empty);

            //once again, just fire and forget, and use the CancellationToken
            //to signal to the "forgotten" async invocation.
            
            connection.ProcessCommandsAsync(cts.Token);
            await connection.Send(new InitiateHandshakeMessage());
            return connection;
        }

        public IEnumerable<Connection> GetConnections(IEnumerable<Guid> ids)
        {
            return ids.Where(_connections.ContainsKey).Select(x => _connections[x]);
        }

       

       
    }
}
