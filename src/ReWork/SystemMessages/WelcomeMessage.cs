using System;

namespace ReWork.SystemMessages
{
    public class WelcomeMessage : IReWorkSystemMessage
    {
        public WelcomeMessage(string message, string clientId)
        {
            Message = message;
            ClientId = clientId;
        }

        public string ClientId { get; }
        public string Message { get;  }
    }
}
