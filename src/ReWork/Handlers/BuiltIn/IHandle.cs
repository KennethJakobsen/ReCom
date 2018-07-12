using System.Threading.Tasks;
using ReWork.Connectivity;

namespace ReWork.Handlers.BuiltIn
{
    internal interface IHandle<T>
    {
        Task Handle(T message, Connection connection);
    }
}