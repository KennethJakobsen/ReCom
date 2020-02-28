using System;
using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.Handlers;
using ReCom.Test.Common.Domain;

namespace ReCom.Test.Application.Server.Handlers
{
    public class PingPongCommandHandler : ICommand<PingPongCommand>
    {

        public async Task Handle(PingPongCommand command, Connection connection)
        {
            Console.WriteLine($"client with id {connection.ClientId} says: {command.Message}");
            await connection.Send(new PingPongCommand() {Message = "Pong!"});
        }
    }
}
