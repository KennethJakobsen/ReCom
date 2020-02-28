namespace ReCom.SystemMessages
{
    public class NotAuthorizedMessage : IReWorkSystemMessage
    {
        public NotAuthorizedMessage(string message)
        {
            Message = message;
        }
        public string Message { get; }
    }
}
