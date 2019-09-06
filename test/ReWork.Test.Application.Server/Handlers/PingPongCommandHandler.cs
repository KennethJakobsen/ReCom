using System;
using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.Handlers;
using ReWork.Test.Common.Domain;

namespace ReWork.Test.Application.Server.Handlers
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
