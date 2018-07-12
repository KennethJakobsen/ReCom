using System.Threading.Tasks;
using ReWork.Connectivity;

namespace ReWork.Handlers
{
    // ReSharper disable once TypeParameterCanBeVariant
    public interface ICommand<T>
    {
        Task Handle(T command, Connection connection);
    }
}
