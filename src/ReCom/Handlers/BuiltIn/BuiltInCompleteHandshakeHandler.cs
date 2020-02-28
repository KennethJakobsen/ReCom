using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.SystemMessages;

namespace ReCom.Handlers.BuiltIn
{
    public class BuiltInCompleteHandshakeHandler : IHandle<CompleteHandshakeMessage>
    {
        public Task Handle(CompleteHandshakeMessage message, Connection connection)
        {
            connection.UpdateId(message.ConnectionId);
            return Task.CompletedTask;
        }
    }
}
