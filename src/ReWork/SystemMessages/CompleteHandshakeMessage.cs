using System;

namespace ReWork.SystemMessages
{
    public class CompleteHandshakeMessage : IReWorkSystemMessage
    {
        public CompleteHandshakeMessage(string connectionId)
        {
            ConnectionId = connectionId;
        }

        public string ConnectionId { get;  }
    }
}
