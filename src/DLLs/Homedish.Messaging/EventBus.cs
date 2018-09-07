using System;
using System.Threading;
using System.Threading.Tasks;
using Homedish.Aws.SNS;
using Homedish.Aws.SQS;
using Homedish.Aws.SQS.Models;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Homedish.Messaging
{
    public class EventBus : IPublisher, IListener
    {
        private readonly ILogger _logger;
        private readonly IEventBusInitializer _eventBusInitializer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISnsOperations _snsOperations;
        private readonly ISqsOperations _sqsOperations;

        public EventBus(ILogger logger, ISnsOperations snsOperations, ISqsOperations sqsOperations, IEventBusInitializer eventBusInitializer, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _snsOperations = snsOperations;
            _sqsOperations = sqsOperations;
            _eventBusInitializer = eventBusInitializer;
            _serviceProvider = serviceProvider;

            //Ioc.InjectNonParamaterableServices();
            //Ioc.InjectParamaterableServices<ILogger>(logger);
            //Ioc.BuildServiceProvider();

            //_snsOperations = Ioc.Services.GetService<ISnsOperations>();
            //_sqsOperations = Ioc.Services.GetService<ISqsOperations>();
            //_eventBusInitializer = Ioc.Services.GetService<IEventBusInitializer>();
        }

        private async Task<bool> PublishInternalAsync<TEvent>(TEvent @event) where TEvent : Event
        {
            var topicArn = _eventBusInitializer.GetTopicArn<TEvent>();

            return await _snsOperations.Publish(@event, topicArn);
        }

        public async Task<bool> PublishAsync<TEvent>(TEvent content) where TEvent : Event
        {
            if (_eventBusInitializer.IsSuccessfullyInitialized<TEvent>())
            {
                return await PublishInternalAsync(content);
            }

            if (await _eventBusInitializer.SetupMessageBusWithSnsAndSqs<TEvent>())
            {
                return await PublishInternalAsync(content);
            }

            return false;
        }

        private IHandler<TEvent> GetHandler<TEvent>() where TEvent : Event
        {
            var handler = (IHandler<TEvent>)_serviceProvider.GetService(typeof(IHandler<TEvent>));
            if (handler == default(IHandler<TEvent>))
            {
                _logger.Warn($"No handler that implements {typeof(IHandler<TEvent>)} was found for the event {typeof(TEvent)}");
                return null;
            }
            return handler;
        }

        private void ListenToInternal<TEvent>() where TEvent : Event
        {
            var queueUrl = _eventBusInitializer.GetQueueUrl<TEvent>();

            Task.Run(async () =>
            {
                //while (true)
                //{
                    var eventsReceived = await _sqsOperations.Dequeue(queueUrl);

                    foreach (var eventReceivedRaw in eventsReceived)
                    {
                        try
                        {
                            var eventReceived = JsonConvert.DeserializeObject<SqsPulledObject>(eventReceivedRaw);
                            var handler = GetHandler<TEvent>();

                            await handler.HandleAsync(JsonConvert.DeserializeObject<TEvent>(eventReceived.Message));
                        }
                        catch (Exception e)
                        {
                        }
                        
                    }

                    Thread.Sleep(1000);
                //}
            });
        }

        public IListener ListenTo<TEvent>() where TEvent : Event
        {
            if (_eventBusInitializer.IsSuccessfullyInitialized<TEvent>())
            {
                ListenToInternal<TEvent>();
            }

            var setupTask = _eventBusInitializer.SetupMessageBusWithSnsAndSqs<TEvent>();
            setupTask.Wait();

            if (setupTask.Result)
            {
                ListenToInternal<TEvent>();
            }

            return this;
        }
    }
}
