using System.Threading.Tasks;

namespace Homedish.Events.Contracts
{
    public interface IHandler<in TEvent> where TEvent : Event
    {
        Task<bool> HandleAsync(TEvent message);
    }
}
