namespace ReCom.SystemMessages
{
    public class TimeoutMessage : IReWorkSystemMessage
    {
        public TimeoutMessage(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
