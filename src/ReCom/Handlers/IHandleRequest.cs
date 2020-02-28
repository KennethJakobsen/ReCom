using System.Threading.Tasks;

namespace ReCom.Handlers
{
    public interface IHandleRequest<T>
    {
        Task<string> Handle(T message);
    }
}