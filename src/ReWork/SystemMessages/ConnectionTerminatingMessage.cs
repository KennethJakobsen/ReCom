namespace ReWork.SystemMessages
{
    public class ConnectionTerminatingMessage
    {
        public ConnectionTerminatingMessage(string message = null)
        {
            if (message == null)
                message = "Terminating connection!";
            Message = message;
        }
        public string Message { get; }
    }
}
