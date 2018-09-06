using System;
using System.Threading.Tasks;
using Homedish.Aws.SNS;
using Homedish.Aws.SQS;
using Homedish.Events.Contracts;

namespace Homedish.Messaging
{
    public class MessagingInitializer
    {
        private static MessagingInitializer _instance = null;
        private static readonly object Lock = new object();

        private static ISnsOperations _snsOperations;
        private static ISqsOperations _sqsOperations;

        private bool _isSuccessfullyInitialized = false;

        public static MessagingInitializer GetInstance()
        {
            lock (Lock)
            {
                if (_instance == null)
                {
                    _sqsOperations = new SqsOperations();
                    _snsOperations = new SnsOperations(_sqsOperations);

                    _instance = new MessagingInitializer();
                }
                return _instance;
            }
        }

        public bool IsSuccessfullyInitialized()
        {
            return _isSuccessfullyInitialized;
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
            return await _sqsOperations.GetQueueArn(queueName);
        }

        public async Task<bool> SetupMessageBusWithSnsAndSqs<TEvent>() where TEvent : Event
        {
            var @event = (Event) Activator.CreateInstance<TEvent>();

            var snsTopicName = $"sns-{@event.ChannelName.ToLowerInvariant()}";
            var queueName = $"sqs-{@event.ChannelName.ToLowerInvariant()}";

            var snsTopicArn = await GetTopicArn(snsTopicName);
            var queueUrl = await GetQueueUrl(queueName);

            _isSuccessfullyInitialized = await _snsOperations.LinkTopicToQueue(snsTopicArn, queueUrl);

            return _isSuccessfullyInitialized;
        }
    }
}

