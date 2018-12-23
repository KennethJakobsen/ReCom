using System;
using System.Collections.Generic;
using System.Text;

namespace ReWork.SystemMessages
{
    public class NotAuthorizedMessage : IReWorkSystemMessage
    {
        public NotAuthorizedMessage(string message)
        {
            Message = message;
        }
        public string Message { get; }
    }
}
