using System;
using System.Threading;
using System.Threading.Tasks;
using ReWork.Config.Roles;

namespace ReWork.Connectivity
{
    public interface IServerTransportManager : IDisposable
    {
        Task StartListeningAsync(ReWorkServerRole role, CancellationToken ctx);
        Task StopListeningAsync();
        Task<ITransportConnection> AcceptClientAsync();
    }
}
