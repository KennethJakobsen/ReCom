using System;
using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.Handlers;
using ReWork.SystemMessages;

namespace ReWork.Test.Application.Client.Handlers
{
    public class WelcomeMessageHandler : ICommand<WelcomeMessage>
    {
        public Task Handle(WelcomeMessage command, Connection connection)
        {
            Console.WriteLine(command.Message);
            Console.WriteLine($"Your connection id is: {connection.ClientId}");
            return Task.CompletedTask;
        }
    }
}
