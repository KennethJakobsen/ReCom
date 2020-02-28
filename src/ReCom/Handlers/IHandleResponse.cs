using System.Threading.Tasks;
using ReCom.Connectivity;

namespace ReCom.Handlers
{
    public interface IHandleResponse
    {
        Task Handle(string response, Connection connection);
    }
}