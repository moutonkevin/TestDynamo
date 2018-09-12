using System;
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
        private readonly IEventBusInitializer _eventBusInitializer;
        private readonly ISnsOperations _snsOperations;
        private readonly ISqsOperations _sqsOperations;
        private readonly ILogger _logger;

        public EventBus(ILogger logger)
        {
            Ioc.InjectNonParamaterableServices();
            Ioc.InjectParamaterableServices(logger);
            Ioc.BuildServiceProvider();

            _logger = logger;
            _snsOperations = Ioc.Services.GetService<ISnsOperations>();
            _sqsOperations = Ioc.Services.GetService<ISqsOperations>();
            _eventBusInitializer = Ioc.Services.GetService<IEventBusInitializer>();
        }

        private async Task<bool> PublishInternalAsync<TEvent>(TEvent @event) where TEvent : Event
        {
            var topicArn = _eventBusInitializer.GetTopicArn<TEvent>();

            _logger.Info($"Publishing {typeof(TEvent)}", @event);

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

        private void ListenToInternal<TEvent, THandler>() 
            where TEvent : Event
            where THandler : IHandler<TEvent>
        {
            var queueUrl = _eventBusInitializer.GetQueueUrl<TEvent>();
            var handler = Activator.CreateInstance<THandler>();

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var eventsReceived = await _sqsOperations.Dequeue(queueUrl);

                    foreach (var eventReceivedRaw in eventsReceived)
                    {
                        _logger.Info($"Received {typeof(TEvent)}", eventReceivedRaw.Body);

                        var eventReceived = JsonConvert.DeserializeObject<SqsPulledObject>(eventReceivedRaw.Body);

                        try
                        {
                            var isHandledSuccessfully = await handler.HandleAsync(JsonConvert.DeserializeObject<TEvent>(eventReceived.Message));

                            if (isHandledSuccessfully)
                            {
                                #pragma warning disable CS4014
                                Task.Run(() => _sqsOperations.Delete(queueUrl, eventReceivedRaw));
                                #pragma warning restore CS4014
                            }
                            else
                            {
                                //TODO
                                //Dead letter queue
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.Error($"An unexpected error happened when processing the message {typeof(TEvent)}. {e}");
                        }
                    }
                }
            }, 
            _eventBusInitializer.GetCancellationToken<TEvent>().Token, 
            TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public IListener StartListening<TEvent, THandler>() 
            where TEvent : Event 
            where THandler : IHandler<TEvent>
        {
            if (_eventBusInitializer.IsSuccessfullyInitialized<TEvent>())
            {
                ListenToInternal<TEvent, THandler>();
            }

            var setupTask = _eventBusInitializer.SetupMessageBusWithSnsAndSqs<TEvent>();
            setupTask.Wait();

            if (setupTask.Result)
            {
                ListenToInternal<TEvent, THandler>();
            }

            return this;
        }

        public void StopListening<TEvent>() where TEvent : Event
        {
            var cancellationToken = _eventBusInitializer.GetCancellationToken<TEvent>();

            cancellationToken.Cancel();
        }
    }
}
