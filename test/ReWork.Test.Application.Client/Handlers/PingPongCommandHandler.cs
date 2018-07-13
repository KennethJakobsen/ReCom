using System;
using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.Handlers;
using ReWork.Test.Common.Domain;

namespace ReWork.Test.Application.Client.Handlers
{
    public class PingPongCommandHandler : ICommand<PingPongCommand>
    {
        public Task Handle(PingPongCommand command, Connection connection)
        {
            Console.WriteLine(command.Message);
            return Task.CompletedTask;
        }
    }
}
