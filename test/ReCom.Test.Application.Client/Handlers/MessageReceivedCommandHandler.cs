using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.Handlers;
using ReCom.SystemMessages;

namespace ReCom.Test.Application.Client.Handlers
{
    public class MessageReceivedCommandHandler : ICommand<ReceivedMessage>
    {
        public Task Handle(ReceivedMessage command, Connection connection)
        {
            Console.WriteLine($"Server has received: {command.ReceivedMessageId}");
            return Task.CompletedTask;
        }
    }
}
