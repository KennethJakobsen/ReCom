using System.Threading.Tasks;

namespace ReWork.Handlers
{
    public interface IHandleRequest<T>
    {
        Task<string> Handle(T message);
    }
}