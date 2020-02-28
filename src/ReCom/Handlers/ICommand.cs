using System.Threading.Tasks;
using ReCom.Connectivity;

namespace ReCom.Handlers
{
    // ReSharper disable once TypeParameterCanBeVariant
    public interface ICommand<T>
    {
        Task Handle(T command, Connection connection);
    }
}
