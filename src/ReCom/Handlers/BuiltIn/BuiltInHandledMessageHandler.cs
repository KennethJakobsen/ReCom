using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.SystemMessages;

namespace ReCom.Handlers.BuiltIn
{
    public class BuiltInHandledMessageHandler : IHandle<HandledMessage>
    {
        public Task Handle(HandledMessage message, Connection connection)
        {
            connection.MarkAsHandled(message.HandledMessageId);
            return Task.CompletedTask;
        }
    }
}
