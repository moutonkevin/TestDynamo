using System.Threading.Tasks;

namespace Homedish.Events.Contracts
{
    public interface IPublisher
    {
        Task<bool> PublishAsync<TEvent>(TEvent content) where TEvent : Event;
    }
}
