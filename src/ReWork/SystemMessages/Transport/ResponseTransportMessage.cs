using System;

namespace ReWork.SystemMessages.Transport
{
    public class ResponseTransportMessage : ITransportMessages
    {
        public ResponseTransportMessage()
        {
            MessageId = Guid.NewGuid();
        }
        public object Payload { get; set; }
        public Guid MessageId { get; set; }

    }
}
