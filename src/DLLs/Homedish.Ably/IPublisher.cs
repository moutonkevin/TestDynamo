using Homedish.Events.Contracts;
using System.Threading.Tasks;

namespace Homedish.Ably
{
    public interface IPublisher
    {
        Task<bool> PublishAsync<TEvent>(TEvent content) where TEvent : Event;
    }
}
