﻿using System;
using System.Threading.Tasks;
using ReWork.Connectivity;
using ReWork.SystemMessages;

namespace ReWork.Handlers.BuiltIn
{
    public class BuiltInTimeoutMessageHandler : IHandle<TimeoutMessage>
    {
        public Task Handle(TimeoutMessage message, Connection connection)
        {
            Console.WriteLine($"Connection timed out for client: {connection.ClientId}");
            return Task.CompletedTask;
        }
    }
}
