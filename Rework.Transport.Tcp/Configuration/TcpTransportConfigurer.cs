using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Rework.Transport.Tcp.Configuration
{
    public class TcpTransportConfigurer
    {
        internal X509Certificate Certificate { get; private set; }
        internal bool UseTls { get; private set; }
        internal bool IsServer { get; private set; }

        public TcpTransportConfigurer UseTlsClient()
        {
            UseTls = true;
            return this;
        }

        public TcpTransportConfigurer UseTlsServer(X509Certificate certificate)
        {
            UseTls = true;
            IsServer = true;
            return this;
        }
    }
}
