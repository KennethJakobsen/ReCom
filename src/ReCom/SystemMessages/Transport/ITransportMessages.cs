using System;

namespace ReCom.SystemMessages.Transport
{
    public interface ITransportMessages
    {
        Guid MessageId { get; set; }
        object Payload { get; set; }
    }
}
