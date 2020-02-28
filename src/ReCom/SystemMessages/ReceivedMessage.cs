using System;

namespace ReCom.SystemMessages
{
    public class ReceivedMessage : IReWorkSystemMessage
    {
        public Guid ReceivedMessageId{ get; set; }
    }
}
