using System.Net;
using ReCom.Transport.Tcp.Extensions;
using ReCom.Activation;
using ReCom.Config;
using ReCom.Config.Roles;
using ReCom.Handlers;
using ReCom.SystemMessages;
using ReCom.Test.Application.Server.Handlers;
using ReCom.Test.Common.Domain;

namespace ReCom.Test.Application.Server
{
    class Program
    {
        public static void Main()
        {
            var activator = new DefaultActivator();
            activator.Register<ICommand<PingPongCommand>, PingPongCommandHandler>();
            activator.Register<ICommand<InitiateHandshakeMessage>, InitiateHandshakeMessageHandler>();
            Configure.With(activator)
                .Transport(t => t.UseTcpTransport())
                .Start(new ReWorkServerRole(IPAddress.Any, 13000));
        }
    }
}