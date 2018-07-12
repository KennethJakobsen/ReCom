using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.SystemMessages;

namespace ReWork.Handlers.BuiltIn
{
    public class BuiltInWelcomeMessageHandler : IHandle<WelcomeMessage>
    {
        public virtual Task Handle(WelcomeMessage command, Connection connection)
        {
            connection.UpdateId(command.ClientId);
            return Task.CompletedTask;
        }
    }
}
