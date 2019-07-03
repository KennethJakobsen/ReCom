using System;

namespace ReWork.SystemMessages
{
    public class HandledMessage : IReWorkSystemMessage
    {
        public Guid HandledMessageId { get; set; }
    }
}
