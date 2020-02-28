using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ReCom.Transport.Tcp.Protocol
{
    public interface IProtocol
    {
        Task<byte[]> ReadCommandFromStream(Stream stream, CancellationToken ct);
        Task WriteCommandToStream(byte[] data, Stream stream);
    }
}