using System;

namespace ReCom.SystemMessages.Transport
{
    public class CommandTransportMessage : ITransportMessages
    {
        public CommandTransportMessage()
        {
            MessageId = Guid.NewGuid();
        }
        public object Payload { get; set; }
        public bool RequiresReceivedFeedback { get; set; }
        public bool RequiresHandledFeedback { get; set; }
        public Guid MessageId { get; set; }

    }
}
