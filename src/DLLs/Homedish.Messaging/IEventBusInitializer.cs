using System.Threading;
using System.Threading.Tasks;
using Homedish.Events.Contracts;

namespace Homedish.Messaging
{
    public interface IEventBusInitializer
    {
        bool IsSuccessfullyInitialized<TEvent>() where TEvent : Event;
        string GetTopicArn<TEvent>() where TEvent : Event;
        string GetQueueUrl<TEvent>() where TEvent : Event;
        CancellationTokenSource GetCancellationToken<TEvent>() where TEvent : Event;
        Task<bool> SetupMessageBusWithSnsAndSqs<TEvent>(EventConfiguration configuration) where TEvent : Event;
    }
}