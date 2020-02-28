using System;
using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.SystemMessages;

namespace ReCom.Handlers.BuiltIn
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
