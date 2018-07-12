using System.Threading.Tasks;
using ReWork.Config.Roles;

namespace ReWork.Connectivity
{
    public interface IConnectionManager
    {
        Task StartListening(ReWorkServerRole role);
        Connection Connect(ReWorkClientRole role);
    }
}