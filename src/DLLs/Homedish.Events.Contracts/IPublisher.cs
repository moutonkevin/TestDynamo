using System.Threading.Tasks;

namespace Homedish.Events.Contracts
{
    public interface IPublisher
    {
        Task<bool> PublishAsync<TEvent>(TEvent content, EventConfiguration configuration = null) where TEvent : Event;
    }
}
