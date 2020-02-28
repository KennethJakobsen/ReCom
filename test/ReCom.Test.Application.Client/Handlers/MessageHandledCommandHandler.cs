using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.Handlers;
using ReCom.SystemMessages;

namespace ReCom.Test.Application.Client.Handlers
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
