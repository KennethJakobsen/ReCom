using System;

namespace ReCom.SystemMessages
{
    public class HandledMessage : IReWorkSystemMessage
    {
        public Guid HandledMessageId { get; set; }
    }
}
