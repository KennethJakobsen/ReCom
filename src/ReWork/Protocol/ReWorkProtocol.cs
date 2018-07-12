using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ReWork.Protocol
{
    internal class ReWorkProtocol : IProtocol
    {
        public async Task<byte[]> ReadCommandFromStream(Stream stream, CancellationToken ct)
        {
            if (!stream.CanRead) return null;
            var lengthBuffer = new byte[sizeof(int)];
            var recv = await stream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length, ct);

            if (recv != sizeof(int)) return null;

            var commandLen = BitConverter.ToInt32(lengthBuffer, 0);
            var commandBuffer = new byte[commandLen];
            recv = await stream.ReadAsync(commandBuffer, 0, commandBuffer.Length, ct);
            
            return recv == commandLen ? commandBuffer : null;
        }

        public async Task WriteCommandToStream(byte[] data, Stream stream)
        {
            var length = BitConverter.GetBytes(data.Length);
            var buffer = new byte[sizeof(int) + data.Length + 1];
            for (var i = 0; i < sizeof(int); i++)
            {
                buffer[i] = length[i];
            }
            for (var i = 0; i < data.Length; i++)
            {
                buffer[i + sizeof(int)] = data[i];
            }
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
