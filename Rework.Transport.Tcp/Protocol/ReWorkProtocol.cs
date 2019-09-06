using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rework.Transport.Tcp.Protocol
{
    internal enum MessageType
    {
        Message = 1,
        BigData = 2,
        SystemOnly = 3
    }
    internal class ReWorkTcpProtocol : IProtocol
    {
        public async Task<byte[]> ReadCommandFromStream(Stream stream, CancellationToken ct)
        {
            if (!stream.CanRead) return null;
            var type = new byte[1];
            await stream.ReadAsync(type, 0, 1, ct);
            var typeEnum = (MessageType) type[0];

            byte[] data = null;
            switch (typeEnum)
            {
                case MessageType.Message:
                    data = await ReceiveMessage(stream, ct);
                    break;
                case MessageType.BigData:
                    break;
                case MessageType.SystemOnly:
                    break;
            }
            return data;
        }



        private static async Task<byte[]> ReceiveMessage(Stream stream, CancellationToken ct)
        {
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
            var buffer = new byte[1 + sizeof(int) + data.Length];
            buffer[0] = (byte) MessageType.Message;
            for (var i = 0; i < sizeof(int); i++)
            {
                buffer[i+1] = length[i];
            }
            for (var i = 0; i < data.Length; i++)
            {
                buffer[i + sizeof(int)+1] = data[i];
            }
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
