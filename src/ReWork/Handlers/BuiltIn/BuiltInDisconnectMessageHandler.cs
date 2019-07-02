using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.SystemMessages;

namespace ReWork.Handlers.BuiltIn
{
    internal class BuiltInDisconnectMessageHandler : IHandle<DisconnectMessage>
    {
        public Task Handle(DisconnectMessage message, Connection connection)
        {
            Console.WriteLine($"Remote connection {connection.ClientId} has disconnected!");
            return Task.CompletedTask;
        }
    }
}
