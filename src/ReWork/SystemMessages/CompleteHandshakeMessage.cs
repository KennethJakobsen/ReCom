using System;
using System.Collections.Generic;
using System.Text;

namespace ReWork.SystemMessages
{
    public class CompleteHandshakeMessage : IReWorkSystemMessage
    {
        public CompleteHandshakeMessage(Guid connectionId)
        {
            ConnectionId = connectionId;
        }

        public Guid ConnectionId { get;  }
    }
}
