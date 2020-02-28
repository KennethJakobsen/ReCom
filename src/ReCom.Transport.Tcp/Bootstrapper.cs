using ReCom.Transport.Tcp.Bson;
using ReCom.Transport.Tcp.Protocol;
using ReCom.Transport.Tcp.Transport;
using ReCom.Activation;
using ReCom.Connectivity;

namespace ReCom.Transport.Tcp
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
