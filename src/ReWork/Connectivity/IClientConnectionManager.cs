using System.Threading.Tasks;
using ReWork.Config.Roles;

namespace ReWork.Connectivity
{
    public interface IClientConnectionManager
    {
        Task<Connection> Connect(ReWorkClientRole role);
    }
}