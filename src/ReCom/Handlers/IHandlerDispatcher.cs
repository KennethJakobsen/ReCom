using System.Threading.Tasks;
using ReCom.Connectivity;

namespace ReCom.Handlers
{
    public interface IHandlerDispatcher
    {
        Task Execute(object command, Connection connection);
    }
}