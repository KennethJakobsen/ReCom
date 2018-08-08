using ReWork.SystemMessages.Transport;

namespace ReWork.Bson
{
    public interface ICommandConverter
    {
        TransportMessage Deserialize(byte[] data);
        byte[] Serialize(object obj);
    }
}