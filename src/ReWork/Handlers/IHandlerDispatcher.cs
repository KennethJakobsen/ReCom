using System.Threading.Tasks;
using ReWork.Connectivity;

namespace ReWork.Handlers
{
    public interface IHandlerDispatcher
    {
        Task Execute(object command, Connection connection);
    }
}