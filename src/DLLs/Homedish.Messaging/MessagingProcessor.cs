using System;
using System.Threading.Tasks;
using Homedish.Aws.SNS;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Homedish.Messaging
{
    public class MessagingProcessor : IMessagingProcessor
    {
        private readonly IMessagingInitializer _messagingInitializer;
        private readonly ISnsOperations _snsOperations;

        public MessagingProcessor(ILogger logger)
        {
            Ioc.InjectNonParamaterableServices();
            Ioc.InjectParamaterableServices<ILogger>(logger);
            Ioc.BuildServiceProvider();

            _snsOperations = Ioc.Services.GetService<ISnsOperations>();
            _messagingInitializer = Ioc.Services.GetService<IMessagingInitializer>();
        }

        public async Task<bool> Send<TEvent>(TEvent @event) where TEvent : Event
        {
            if (_messagingInitializer.IsSuccessfullyInitialized<TEvent>())
            {
                return await SendInternal(@event);
            }

            if (await _messagingInitializer.SetupMessageBusWithSnsAndSqs<TEvent>())
            {
                return await SendInternal(@event);
            }

            return false;
        }

        private async Task<bool> SendInternal<TEvent>(TEvent @event) where TEvent : Event
        {
            var topicArn = _messagingInitializer.GetTopicArn<TEvent>();

            return await _snsOperations.Publish(@event, topicArn);
        }

        public async Task Receive<TEvent>() where TEvent : Event
        {
            throw new NotImplementedException();
        }
    }
}
