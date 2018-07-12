using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ReWork.Protocol
{
    public interface IProtocol
    {
        Task<byte[]> ReadCommandFromStream(Stream stream, CancellationToken ct);
        Task WriteCommandToStream(byte[] data, Stream stream);
    }
}