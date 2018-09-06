using System;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Homedish.Aws.SQS;

namespace Homedish.Aws.SNS
{
    public class SnsOperations : ISnsOperations
    {
        private readonly ISqsOperations _sqsOperations;
        private readonly IAmazonSimpleNotificationService _client = new AmazonSimpleNotificationServiceClient();

        public SnsOperations(ISqsOperations sqsOperations)
        {
            _sqsOperations = sqsOperations;
        }

        public async Task<string> TopicExists(string name)
        {
            var topic = await _client.FindTopicAsync(name);

            return topic.TopicArn;
        }

        public async Task<string> CreateTopic(string name)
        {
            try
            {
                var topic = await _client.CreateTopicAsync(new CreateTopicRequest
                {
                    Name = name
                });

                return topic.TopicArn;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> LinkTopicToQueue(string topicArn, string queueUrl)
        {
            try
            {
                var sqsClient = _sqsOperations.GetClient();

                return !string.IsNullOrWhiteSpace(await _client.SubscribeQueueAsync(topicArn, sqsClient, queueUrl));
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
