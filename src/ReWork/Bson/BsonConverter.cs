using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace ReWork.Bson
{
    internal class BsonConverter : ICommandConverter
    {
        public  object Deserialize(byte[] data)
        {
            if (data == null) return null;

            using (var ms = new MemoryStream(data))
            using (var reader = new BsonDataReader(ms))
            {
                var serializer = new JsonSerializer {TypeNameHandling = TypeNameHandling.All};

                var obj = serializer.Deserialize<object>(reader);
                return obj;
            }
        }

        public  byte[] Serialize(object obj)
        {
            using (var ms = new MemoryStream())
            using (var writer = new BsonDataWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.All;

                serializer.Serialize(writer, obj);

                return ms.ToArray();
            }
        }
    }
}