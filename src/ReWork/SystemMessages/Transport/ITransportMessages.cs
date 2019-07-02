using System;
using System.Collections.Generic;
using System.Text;

namespace ReWork.SystemMessages.Transport
{
    internal interface ITransportMessages
    {
        Guid MessageId { get; set; }
        object Payload { get; set; }
    }
}
