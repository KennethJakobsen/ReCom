using ReWork.SystemMessages.Transport;

namespace Rework.Transport.Tcp.Bson
{
    public interface ICommandConverter
    {
        ITransportMessages Deserialize(byte[] data);
        byte[] Serialize(object obj);
    }
}