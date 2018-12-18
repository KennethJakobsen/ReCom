using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.Handlers;
using ReWork.SystemMessages;

namespace ReWork.Test.Application.Client.Handlers
{
    public class MessageHandledCommandHandler : ICommand<HandledMessage>
    {
        public Task Handle(HandledMessage command, Connection connection)
        {
            Console.WriteLine($"Server has handled: {command.HandledMessageId}");
            return Task.CompletedTask;
        }
    }
}
