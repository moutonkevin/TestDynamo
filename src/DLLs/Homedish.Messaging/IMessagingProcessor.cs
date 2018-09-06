using System.Threading.Tasks;
using Homedish.Events.Contracts;

namespace Homedish.Messaging
{
    public interface IMessagingProcessor
    {
        Task<bool> Send<TEvent>(TEvent @event) where TEvent : Event;
        Task Receive<TEvent>() where TEvent : Event;
    }
}
