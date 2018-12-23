using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.SystemMessages;

namespace ReWork.Handlers.BuiltIn
{
    public class BuiltInInitiateHandshakeHandler : IHandle<InitiateHandshakeMessage>
    {
        public async Task Handle(InitiateHandshakeMessage message, Connection connection)
        {
            connection.Authorize();
            await connection.Send(new CompleteHandshakeMessage(connection.ClientId));

        }
    }
}
