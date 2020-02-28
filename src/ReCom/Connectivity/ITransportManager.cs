using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReCom.Config.Roles;

namespace ReCom.Connectivity
{
    public interface ITransportManager : IDisposable
    {
        Task StartListeningAsync(ReWorkServerRole role, CancellationToken ctx);
        Task StopListeningAsync();
        Task<ITransportConnection> AcceptClientAsync();

        Task<ITransportConnection> ConnectAsync(ReWorkClientRole role, CancellationToken ctx);
    }
}
