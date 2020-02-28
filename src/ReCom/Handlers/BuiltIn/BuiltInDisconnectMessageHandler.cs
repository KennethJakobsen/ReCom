using System;
using System.Threading.Tasks;
using ReCom.Connectivity;
using ReCom.SystemMessages;

namespace ReCom.Handlers.BuiltIn
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
