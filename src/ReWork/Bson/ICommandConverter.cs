using ReWork.SystemMessages.Transport;

namespace ReWork.Bson
{
    internal interface ICommandConverter
    {
        ITransportMessages Deserialize(byte[] data);
        byte[] Serialize(object obj);
    }
}