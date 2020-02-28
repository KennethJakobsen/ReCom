using System.Threading.Tasks;
using ReCom.Config.Roles;

namespace ReCom.Connectivity
{
    public interface IConnectionManager
    {
        Task StartListening(ReWorkServerRole role);
        Task<Connection> Connect(ReWorkClientRole role);
    }
}