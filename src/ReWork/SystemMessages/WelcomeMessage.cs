using System;

namespace ReWork.SystemMessages
{
    public class WelcomeMessage : IReWorkSystemMessage
    {
        public WelcomeMessage(string message, Guid clientId)
        {
            Message = message;
            ClientId = clientId;
        }

        public Guid ClientId { get; }
        public string Message { get;  }
    }
}
