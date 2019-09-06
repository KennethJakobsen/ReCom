using System.Threading.Tasks;
using ReWork.Config.Roles;

namespace ReWork.Connectivity
{
    public interface IServerConnectionManager 
    {
        Task StartListening(ReWorkServerRole role);
    }
}