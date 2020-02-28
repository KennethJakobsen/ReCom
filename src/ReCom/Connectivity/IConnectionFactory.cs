namespace ReCom.Connectivity
{
    public interface IConnectionFactory
    {
        Connection Create(string clientId, ITransportConnection connection, INotifyTermination terminator);
    }
}