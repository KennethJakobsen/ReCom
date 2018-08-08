using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.SystemMessages;

namespace ReWork.Handlers.BuiltIn
{
    public class BuiltInReceivedMessageHandler : IHandle<ReceivedMessage>
    {
        public Task Handle(ReceivedMessage message, Connection connection)
        {
            connection.MarkAsReceived(message.ReceivedMessageId);
            return Task.CompletedTask;
        }
    }
}
