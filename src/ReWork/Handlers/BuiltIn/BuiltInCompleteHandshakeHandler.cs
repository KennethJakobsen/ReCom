﻿using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.SystemMessages;

namespace ReWork.Handlers.BuiltIn
{
    public class BuiltInCompleteHandshakeHandler : IHandle<CompleteHandshakeMessage>
    {
        public Task Handle(CompleteHandshakeMessage message, Connection connection)
        {
            connection.UpdateId(message.ConnectionId);
            return Task.CompletedTask;
        }
    }
}
