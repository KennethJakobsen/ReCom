using ReCom.SystemMessages.Transport;

namespace ReCom.Transport.Tcp.Bson
{
    public interface ICommandConverter
    {
        ITransportMessages Deserialize(byte[] data);
        byte[] Serialize(object obj);
    }
}