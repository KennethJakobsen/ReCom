﻿using System;

namespace ReWork.SystemMessages.Transport
{
    public class TransportMessage
    {
        public object Payload { get; set; }
        public bool RequiresReceivedFeedback { get; set; }
        public bool RequiresHandledFeedback { get; set; }
        public Guid MessageId { get; set; }

    }
}
