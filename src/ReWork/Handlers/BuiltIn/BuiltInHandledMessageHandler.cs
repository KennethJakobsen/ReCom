using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.SystemMessages;

namespace ReWork.Handlers.BuiltIn
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
