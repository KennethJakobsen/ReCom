using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.SystemMessages;

namespace ReCom.Handlers.BuiltIn
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
