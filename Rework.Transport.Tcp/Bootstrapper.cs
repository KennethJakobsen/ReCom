using Rework.Transport.Tcp.Bson;
using Rework.Transport.Tcp.Protocol;
using Rework.Transport.Tcp.Transport;
using ReWork.Activation;
using ReWork.Connectivity;

namespace Rework.Transport.Tcp
{
    public class Bootstrapper
    {
        

        public static void RegisterServices(IActivator activator)
        {
            activator.Register<ICommandConverter, BsonConverter>();
            activator.Register<IProtocol, ReWorkProtocol>();
            activator.Register<ITransportManager, ReworkTcpTransportManager>();
        }
    }
}
