using System.Threading.Tasks;
using ReCom.Connectivity;

namespace ReCom.Handlers.BuiltIn
{
    internal interface IHandle<T>
    {
        Task Handle(T message, Connection connection);
    }
}