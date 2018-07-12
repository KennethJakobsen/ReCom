using System;
using System.Collections.Generic;
using System.IO;
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
                //just fire and forget. We break from the "forgotten" async loops
                //in AcceptClientsAsync using a CancellationToken from `cts`
                await AcceptClientsAsync(listener, cts.Token);
            }
            finally
            {
                cts.Cancel();
                listener.Stop();
            }
        }

        public Connection Connect(ReWorkClientRole role)
        {
            var cts = new CancellationTokenSource();
            var client = new TcpClient(role.Host, role.Port);
            var connection = _factory.Create(client, Guid.Empty);

            ProcessCommandsAsync(connection, cts.Token);
            return connection;
        }

        public IEnumerable<Connection> GetConnections(IEnumerable<Guid> ids)
        {
            return ids.Where(_connections.ContainsKey).Select(x => _connections[x]);
        }

        private async Task AcceptClientsAsync(TcpListener listener, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync()
                    .ConfigureAwait(false);
                var connection = _factory.Create(client, Guid.NewGuid());
                _connections.Add(connection.ClientId, connection);

                connection.Send(new WelcomeMessage("Welcome to the server", connection.ClientId));

                //once again, just fire and forget, and use the CancellationToken
                //to signal to the "forgotten" async invocation.
                ProcessCommandsAsync(connection, ct);
            }

        }

        private async Task ProcessCommandsAsync(Connection connection, CancellationToken ct)
        {
            //TODO: Implement client connected event
            using (connection)
            {

                //TODO investigate
                //if (amountRead == 0) break; //end of stream.

                while (!ct.IsCancellationRequested)
                {
                    //under some circumstances, it's not possible to detect
                    //a client disconnecting if there's no data being sent
                    //so it's a good idea to give them a timeout to ensure that 
                    //we clean them up.

                    try
                    {
                        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(15), ct);
                        var readTask = _protocol.ReadCommandFromStream(connection.Stream, ct);
                        var completedTask = await Task.WhenAny(timeoutTask, readTask)
                            .ConfigureAwait(false);
                        if (completedTask == timeoutTask)
                        {
                            await connection.Send(new TimeoutMessage("Connection timed out"));
                            break;
                        }

                        //now we know that the amountTask is complete so
                        //we can ask for its Result without blocking
                        var commandBytes = readTask.Result;
                        var command = _commandConverter.Deserialize(commandBytes);

                        if (command != null)
                            await _dispatcher.Execute(command, connection);
                        Thread.Sleep(5);
                    }
                    catch (IOException)
                    {
                        break;
                    }
                }
            }
        }

       
       

    }
}
