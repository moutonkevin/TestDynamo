using System;
using System.Threading.Tasks;
using Homedish.Events.Contracts;

namespace Homedish.Messaging
{
    public class MessagingProcessor : IMessagingProcessor
    {
        private readonly MessagingInitializer _messagingInitializer = MessagingInitializer.GetInstance();

        public async Task Send<TEvent>(TEvent @event) where TEvent : Event
        {
            if (_messagingInitializer.IsSuccessfullyInitialized())
            {
                await SendInternal(@event);
            }
            else
            {
                if (await _messagingInitializer.SetupMessageBusWithSnsAndSqs<TEvent>())
                {
                    await SendInternal(@event);
                }
            }
        }

        private async Task SendInternal<TEvent>(TEvent @event) where TEvent : Event
        {

        }

        public async Task Receive<TEvent>() where TEvent : Event
        {
            throw new NotImplementedException();
        }
    }
}
