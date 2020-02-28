namespace ReCom.Connectivity.Delivery
{
    internal class DeliveryKeeper
    {
        private readonly bool _recieveconfirmation;
        private readonly bool _handleconfirmation;
        private bool _hasBeenReceived = false;
        private bool _hasBeenHandled = false;

        public DeliveryKeeper(object message, bool recieveconfirmation, bool handleconfirmation)
        {
            _recieveconfirmation = recieveconfirmation;
            _handleconfirmation = handleconfirmation;
            Message = message;
        }

        public object Message { get; }

        public void Handle()
        {
            _hasBeenHandled = true;
        }

        public void Receive()
        {
            _hasBeenReceived = true;
        }

        public bool IsDone
        {
            get
            {
                var isOk = true;

                if (_recieveconfirmation && !_hasBeenReceived)
                    isOk = false;
                if (_handleconfirmation && !_hasBeenHandled)
                    isOk = false;
                return isOk;
            }
        }
    }
}
