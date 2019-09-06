using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rework.Transport.Tcp.Bson;
using Rework.Transport.Tcp.Configuration;
using Rework.Transport.Tcp.Protocol;
using ReWork.Config.Roles;
using ReWork.Connectivity;

namespace Rework.Transport.Tcp.Transport
{
    public class ReworkClientTcpTransportManager : IClientTransportManager
    {
        private readonly IProtocol _protocol;
        private readonly ICommandConverter _commandConverter;
        private readonly TcpTransportConfigurer _config;
        private TcpListener _listener;

        public ReworkClientTcpTransportManager(IProtocol protocol, ICommandConverter commandConverter, TcpTransportConfigurer config)
        {
            _protocol = protocol;
            _commandConverter = commandConverter;
            _config = config;
        }
        public void Dispose()
        {
            _listener = null;
        }

       

        public Task<ITransportConnection> ConnectAsync(ReWorkClientRole role, CancellationToken ctx)
        {
            var client = new TcpClient(role.Host, role.Port);
            return Task.FromResult(new ReworkTcpTransport(client, _commandConverter, _protocol, client.GetStream()) as ITransportConnection);
        }
        private Stream XGetStream(TcpClient client)
        {
            if (_config.UseTls && _config.IsServer)
                return new SslStream(client.GetStream(), false);
            else if (!_config.IsServer && _config.UseTls)
                return new SslStream(
                client.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null
                );
            else
                return client.GetStream();
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (certificate.GetSerialNumberString() == _config.Certificate.GetSerialNumberString())
                return false;
            if (sslPolicyErrors != SslPolicyErrors.None)
                return false;

            // Do not allow this client to communicate with unauthenticated servers.
            return true;
        }
    }
    public class ReworkServerTcpTransportManager : IServerTransportManager
    {
        private readonly IProtocol _protocol;
        private readonly ICommandConverter _commandConverter;
        private readonly TcpTransportConfigurer _config;
        private TcpListener _listener;

        public ReworkServerTcpTransportManager(IProtocol protocol, ICommandConverter commandConverter, TcpTransportConfigurer config)
        {
            _protocol = protocol;
            _commandConverter = commandConverter;
            _config = config;
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
            return new ReworkTcpTransport(client, _commandConverter, _protocol, client.GetStream());
        }
        public void Dispose()
        {
            _listener = null;
        }

    }
}
