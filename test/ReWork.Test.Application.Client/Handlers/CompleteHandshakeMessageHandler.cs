using System;
using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.Handlers;
using ReWork.SystemMessages;

namespace ReWork.Test.Application.Client.Handlers
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
