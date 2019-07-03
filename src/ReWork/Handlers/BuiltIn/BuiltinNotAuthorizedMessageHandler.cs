using System;
using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.SystemMessages;

namespace ReWork.Handlers.BuiltIn
{
    public class BuiltinNotAuthorizedMessageHandler : IHandle<NotAuthorizedMessage>
    {
        public Task Handle(NotAuthorizedMessage message, Connection connection)
        {
            Console.Write(message.Message);
            return Task.CompletedTask;
        }
    }
}
