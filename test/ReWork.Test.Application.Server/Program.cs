using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Rework.Transport.Tcp.Extensions;
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
            //X509Certificate cert = X509Certificate.CreateFromCertFile("");
            var activator = new DefaultActivator();
            activator.Register<ICommand<PingPongCommand>, PingPongCommandHandler>();
            activator.Register<ICommand<InitiateHandshakeMessage>, InitiateHandshakeMessageHandler>();
            Configure.With(activator)
                .Transport(t => t.UseTcpTransport())
                .Start(new ReWorkServerRole(IPAddress.Any, 13000));
            Console.WriteLine("Server started - waiting for messages");
        }
    }
}