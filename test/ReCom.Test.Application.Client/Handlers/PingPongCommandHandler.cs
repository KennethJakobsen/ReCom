using System;
using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.Handlers;
using ReCom.Test.Common.Domain;

namespace ReCom.Test.Application.Client.Handlers
{
    public class PingPongCommandHandler : ICommand<PingPongCommand>
    {
        public Task Handle(PingPongCommand command, Connection connection)
        {
            Console.WriteLine("Server says: " + command.Message);
            return Task.CompletedTask;
        }
    }
}
