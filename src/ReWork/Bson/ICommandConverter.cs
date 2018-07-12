namespace ReWork.Bson
{
    public interface ICommandConverter
    {
        object Deserialize(byte[] data);
        byte[] Serialize(object obj);
    }
}