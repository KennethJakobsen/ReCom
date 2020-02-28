using System;
using System.Threading;
using System.Threading.Tasks;
using ReCom.SystemMessages.Transport;

namespace ReCom.Connectivity
{
    public interface ITransportConnection : IDisposable
    {
        Task<ITransportMessages> ReadCommandAsync(CancellationToken ctx);
        Task WriteCommandAsync(ITransportMessages command);
        Task<bool> IsConnectedAsync();
        Task DisconnectAsync();
    }
}