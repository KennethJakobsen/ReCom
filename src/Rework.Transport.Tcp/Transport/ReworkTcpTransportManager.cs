using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rework.Transport.Tcp.Bson;
using Rework.Transport.Tcp.Protocol;
using ReWork.Config.Roles;
using ReWork.Connectivity;

namespace Rework.Transport.Tcp.Transport
{
    public class ReworkTcpTransportManager : ITransportManager
    {
        private readonly IProtocol _protocol;
        private readonly ICommandConverter _commandConverter;
        private TcpListener _listener;

        public ReworkTcpTransportManager(IProtocol protocol, ICommandConverter commandConverter)
        {
            _protocol = protocol;
            _commandConverter = commandConverter;
        }
        public void Dispose()
        {
            _listener = null;
        }

        public Task StartListeningAsync(ReWorkServerRole role, CancellationToken ctx)
        {
            _listener = new TcpListener(role.IpAddress, role.Port);
            _listener.Start();
            return Task.CompletedTask;
        }

        public Task StopListeningAsync()
        {
            _listener.Stop();
            return Task.CompletedTask;
        }

        public async Task<ITransportConnection> AcceptClientAsync()
        {
            var client = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
            return new ReworkTcpTransport(client, _commandConverter, _protocol);
        }

        public Task<ITransportConnection> ConnectAsync(ReWorkClientRole role, CancellationToken ctx)
        {
            var client = new TcpClient(role.Host, role.Port);
            return Task.FromResult(new ReworkTcpTransport(client, _commandConverter, _protocol) as ITransportConnection);
        }
    }
}
