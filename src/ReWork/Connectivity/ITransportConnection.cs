using System;
using System.Threading;
using System.Threading.Tasks;
using ReWork.SystemMessages.Transport;

namespace ReWork.Connectivity
{
    public interface ITransportConnection : IDisposable
    {
        Task<ITransportMessages> ReadCommandAsync(CancellationToken ctx);
        Task WriteCommandAsync(ITransportMessages command);
        Task<bool> IsConnectedAsync();
        Task DisconnectAsync();
    }
}