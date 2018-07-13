using System.Net;
using ReWork.Activation;
using ReWork.Config;
using ReWork.Config.Roles;
using ReWork.Handlers;
using ReWork.SystemMessages;
using ReWork.Test.Application.Server.Handlers;
using ReWork.Test.Common.Domain;

namespace ReWork.Test.Application.Server
{
    class Program
    {
        public static void Main()
        {
            var activator = new DefaultActivator();
            activator.Register<ICommand<PingPongCommand>, PingPongCommandHandler>();
            activator.Register<ICommand<InitiateHandshakeMessage>, InitiateHandshakeMessageHandler>();
            Configure.With(activator).Start(new ReWorkServerRole(IPAddress.Any, 13000));
        }
    }
}