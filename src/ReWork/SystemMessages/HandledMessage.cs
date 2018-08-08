using System;
using System.Collections.Generic;
using System.Text;

namespace ReWork.SystemMessages
{
    public class HandledMessage : IReWorkSystemMessage
    {
        public Guid HandledMessageId { get; set; }
    }
}
