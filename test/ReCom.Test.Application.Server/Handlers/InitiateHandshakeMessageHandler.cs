using System;
using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.Handlers;
using ReCom.SystemMessages;

namespace ReCom.Test.Application.Server.Handlers
{
    public class InitiateHandshakeMessageHandler : ICommand<InitiateHandshakeMessage>
    {
        public Task Handle(InitiateHandshakeMessage command, Connection connection)
        {
            Console.WriteLine("Client connected: " + connection.ClientId);
            return Task.CompletedTask;
        }
    }
}
