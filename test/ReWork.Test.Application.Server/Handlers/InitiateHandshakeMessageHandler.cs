using System;
using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.Handlers;
using ReWork.SystemMessages;

namespace ReWork.Test.Application.Server.Handlers
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
