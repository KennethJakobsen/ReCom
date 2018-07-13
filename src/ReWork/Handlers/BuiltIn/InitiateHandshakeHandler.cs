using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.SystemMessages;

namespace ReWork.Handlers.BuiltIn
{
    public class BuiltInInitiateHandshakeHandler : IHandle<InitiateHandshakeMessage>
    {
        public async Task Handle(InitiateHandshakeMessage message, Connection connection)
        {
            await connection.Send(new CompleteHandshakeMessage(connection.ClientId));
        }
    }
}
