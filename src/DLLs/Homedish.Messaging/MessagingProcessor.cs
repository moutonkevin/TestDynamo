using System.Threading;
using System.Threading.Tasks;
using Homedish.Aws.SNS;
using Homedish.Aws.SQS;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Homedish.Messaging
{
    public class MessagingProcessor : IPublisher, IListener
    {
        private readonly ILogger _logger;
        private readonly IMessagingInitializer _messagingInitializer;
        private readonly ISnsOperations _snsOperations;
        private readonly ISqsOperations _sqsOperations;

        public MessagingProcessor(ILogger logger)
        {
            _logger = logger;

            Ioc.InjectNonParamaterableServices();
            Ioc.InjectParamaterableServices<ILogger>(logger);
            Ioc.BuildServiceProvider();

            _snsOperations = Ioc.Services.GetService<ISnsOperations>();
            _sqsOperations = Ioc.Services.GetService<ISqsOperations>();
            _messagingInitializer = Ioc.Services.GetService<IMessagingInitializer>();
        }

        private async Task<bool> SendInternal<TEvent>(TEvent @event) where TEvent : Event
        {
            var topicArn = _messagingInitializer.GetTopicArn<TEvent>();

            return await _snsOperations.Publish(@event, topicArn);
        }

        private IHandler<TEvent> GetHandler<TEvent>() where TEvent : Event
        {
            var handler = (IHandler<TEvent>)Ioc.Services.GetService(typeof(IHandler<TEvent>));
            if (handler == default(IHandler<TEvent>))
            {
                _logger.Warn($"No handler that implements {typeof(IHandler<TEvent>)} was found for the event {typeof(TEvent)}");
                return null;
            }
            return handler;
        }

        public async Task<bool> PublishAsync<TEvent>(TEvent content) where TEvent : Event
        {
            if (_messagingInitializer.IsSuccessfullyInitialized<TEvent>())
            {
                return await SendInternal(content);
            }

            if (await _messagingInitializer.SetupMessageBusWithSnsAndSqs<TEvent>())
            {
                return await SendInternal(content);
            }

            return false;
        }

        public IListener ListenTo<TEvent>() where TEvent : Event
        {
            var queueUrl = _messagingInitializer.GetTopicArn<TEvent>();

            Task.Run(async () =>
            {
                while (true)
                {
                    var eventsReceived = await _sqsOperations.Dequeue(queueUrl);

                    foreach (var eventReceivedRaw in eventsReceived)
                    {
                        var eventReceived = JsonConvert.DeserializeObject<TEvent>(eventReceivedRaw);
                        var handler = GetHandler<TEvent>();

                        await handler.HandleAsync(eventReceived);
                    }

                    Thread.Sleep(1000);
                }
            });

            return this;
        }
    }
}
