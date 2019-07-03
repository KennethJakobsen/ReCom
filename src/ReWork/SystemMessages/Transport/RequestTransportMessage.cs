﻿using System;

namespace ReWork.SystemMessages.Transport
{
    public class RequestTransportMessage : ITransportMessages
    {
        public RequestTransportMessage()
        {
            MessageId = Guid.NewGuid();
        }
        public object Payload { get; set; }
        public Guid MessageId { get; set; }

    }
}
