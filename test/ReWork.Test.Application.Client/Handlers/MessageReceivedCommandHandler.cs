using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.Handlers;
using ReWork.SystemMessages;

namespace ReWork.Test.Application.Client.Handlers
{
    public class MessageReceivedCommandHandler : ICommand<ReceivedMessage>
    {
        public Task Handle(ReceivedMessage command, Connection connection)
        {
            Console.WriteLine($"Message Received: {command.ReceivedMessageId}");
            return Task.CompletedTask;
        }
    }
}
