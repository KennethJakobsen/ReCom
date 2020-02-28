using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.SystemMessages;

namespace ReCom.Handlers.BuiltIn
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
