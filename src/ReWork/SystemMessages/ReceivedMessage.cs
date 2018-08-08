using System;

namespace ReWork.SystemMessages
{
    public class ReceivedMessage : IReWorkSystemMessage
    {
        public Guid ReceivedMessageId{ get; set; }
    }
}
