using Rework.Transport.Tcp.Bson;
using Rework.Transport.Tcp.Configuration;
using Rework.Transport.Tcp.Protocol;
using Rework.Transport.Tcp.Transport;
using ReWork.Activation;
using ReWork.Connectivity;

namespace Rework.Transport.Tcp
{
    public class Bootstrapper
    {
        

        public static void RegisterServices(IActivator activator, TcpTransportConfigurer config)
        {
            activator.Register<ICommandConverter, BsonConverter>();
            activator.Register<IProtocol, ReWorkTcpProtocol>();
            activator.Register<IServerTransportManager, ReworkServerTcpTransportManager>();
            activator.Register<IClientTransportManager, ReworkClientTcpTransportManager>();
            activator.Register<TcpTransportConfigurer>(config);
        }
    }
}
