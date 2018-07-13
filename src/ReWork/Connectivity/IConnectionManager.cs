using System.Threading.Tasks;
using ReWork.Config.Roles;

namespace ReWork.Connectivity
{
    public interface IConnectionManager
    {
        Task StartListening(ReWorkServerRole role);
        Task<Connection> Connect(ReWorkClientRole role);
    }
}