using System;

namespace ReWork.SystemMessages.Transport
{
    public interface ITransportMessages
    {
        Guid MessageId { get; set; }
        object Payload { get; set; }
    }
}
