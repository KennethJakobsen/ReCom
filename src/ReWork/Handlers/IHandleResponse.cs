using System.Threading.Tasks;
using ReWork.Connectivity;

namespace ReWork.Handlers
{
    public interface IHandleResponse
    {
        Task Handle(string response, Connection connection);
    }
}