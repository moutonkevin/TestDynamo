using System.Threading.Tasks;
using Homedish.Events.Contracts;

namespace Homedish.Messaging
{
    public interface IMessagingInitializer
    {
        bool IsSuccessfullyInitialized<TEvent>() where TEvent : Event;
        string GetTopicArn<TEvent>() where TEvent : Event;
        Task<bool> SetupMessageBusWithSnsAndSqs<TEvent>() where TEvent : Event;
    }
}