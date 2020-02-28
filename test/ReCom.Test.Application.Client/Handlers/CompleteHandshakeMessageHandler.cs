using System;
using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.Handlers;
using ReCom.SystemMessages;

namespace ReCom.Test.Application.Client.Handlers
{
    public class CompleteHandshakeMessageHandler : ICommand<CompleteHandshakeMessage>
    {
        public Task Handle(CompleteHandshakeMessage command, Connection connection)
        {
            Console.WriteLine("Connected to server.");
            Console.WriteLine($"Your connection id is: {connection.ClientId}");
            return Task.CompletedTask;
        }
    }
}
