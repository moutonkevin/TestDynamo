using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homedish.Aws.SNS;
using Homedish.Aws.SQS;
using Homedish.Events.Contracts;
using Homedish.Messaging.Models;

namespace Homedish.Messaging
{
    public class MessagingInitializer : IMessagingInitializer
    {
        private readonly ISnsOperations _snsOperations;
        private readonly ISqsOperations _sqsOperations;

        public MessagingInitializer(ISnsOperations snsOperation, ISqsOperations sqsOperations)
        {
            _snsOperations = snsOperation;
            _sqsOperations = sqsOperations;
        }

        private readonly IList<MessagingInitializerModel> _events = new List<MessagingInitializerModel>();

        public bool IsSuccessfullyInitialized<TEvent>() where TEvent : Event
        {
            var @event = _events.FirstOrDefault(e => e.EventType == typeof(TEvent));
            if (@event == null)
            {
                return false;
            }

            return @event.IsSuccessfullyInitialized;
        }

        public string GetTopicArn<TEvent>() where TEvent : Event
        {
            var @event = _events.FirstOrDefault(e => e.EventType == typeof(TEvent));

            return @event?.TopicArn;
        }

        public string GetQueueUrl<TEvent>() where TEvent : Event
        {
            var @event = _events.FirstOrDefault(e => e.EventType == typeof(TEvent));

            return @event?.QueueUrl;
        }

        private async Task<string> GetTopicArn(string snsTopicName)
        {
            var topicArn = await _snsOperations.TopicExists(snsTopicName);
            if (topicArn == null)
            {
                return await _snsOperations.CreateTopic(snsTopicName);
            }
            return topicArn;
        }

        private async Task<string> GetQueueUrl(string queueName)
        {
            if (!await _sqsOperations.QueueExists(queueName))
            {
                return await _sqsOperations.CreateQueue(queueName, 42000);
            }
            return await _sqsOperations.GetQueueUrl(queueName);
        }

        private async Task<bool> LinkTopicToQueue(string topicArn, string queueUrl)
        {
            if (await _snsOperations.IsTopicLinkedToQueue(topicArn, queueUrl))
            {
                return true;
            }
            return await _snsOperations.LinkTopicToQueue(topicArn, queueUrl);
        }

        public async Task<bool> SetupMessageBusWithSnsAndSqs<TEvent>() where TEvent : Event
        {
            var @event = (Event) Activator.CreateInstance<TEvent>();

            var snsTopicName = EventUtils.GetTopicName(@event);
            var queueName = EventUtils.GetQueueName(@event);

            var snsTopicArn = await GetTopicArn(snsTopicName);
            var queueUrl = await GetQueueUrl(queueName);

            var isSuccessfullyInitialized = await LinkTopicToQueue(snsTopicArn, queueUrl);

            _events.Add(new MessagingInitializerModel
            {
                TopicArn = snsTopicArn,
                EventType = typeof(TEvent),
                IsSuccessfullyInitialized = isSuccessfullyInitialized,
                QueueUrl = queueUrl
            });

            return isSuccessfullyInitialized;
        }
    }
}

