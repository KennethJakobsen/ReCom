using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReCom.Transport.Tcp.Bson;
using ReCom.Transport.Tcp.Protocol;
using ReCom.Connectivity;
using ReCom.SystemMessages;
using ReCom.SystemMessages.Transport;

namespace ReCom.Transport.Tcp.Transport
{
    public class ReworkTcpTransport : ITransportConnection
    {
        private readonly TcpClient _client;
        private readonly ICommandConverter _commandConverter;
        private readonly IProtocol _protocol;
        private readonly NetworkStream _stream;

        public ReworkTcpTransport(TcpClient client, ICommandConverter commandConverter, IProtocol protocol)
        {
            _client = client;
            _stream = _client.GetStream();
            _commandConverter = commandConverter;
            _protocol = protocol;
        }
        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<ITransportMessages> ReadCommandAsync(CancellationToken ct)
        {
            var timeoutTask = Task.Delay(TimeSpan.FromHours(2), ct);
            var readTask = _protocol.ReadCommandFromStream(_stream, ct);
            var completedTask = await Task.WhenAny(timeoutTask, readTask)
                .ConfigureAwait(false);
            if (completedTask == timeoutTask)
            {
                return new CommandTransportMessage() { Payload = new TimeoutMessage("Connection timed out") };
            }

            //now we know that the amountTask is complete so
            //we can ask for its Result without blocking
            var commandBytes = readTask.Result;
            return _commandConverter.Deserialize(commandBytes);
        }

        public async Task WriteCommandAsync(ITransportMessages command)
        {
            var commandBytes = _commandConverter.Serialize(command);
            await _protocol.WriteCommandToStream(commandBytes, _stream);
        }

        public Task<bool> IsConnectedAsync()
        {
             return Task.FromResult(_client.Connected);
        }

        public Task DisconnectAsync()
        {
            _client.Dispose();
            return Task.CompletedTask;
        }
    }
}
