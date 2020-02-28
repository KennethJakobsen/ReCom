using System.Net;

namespace ReCom.Config.Roles
{
    public class ReWorkServerRole : IReWorkRole
    {
        public ReWorkServerRole(IPAddress ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
        }

        public IPAddress IpAddress { get; }
        public int Port { get;  }
    }
}
