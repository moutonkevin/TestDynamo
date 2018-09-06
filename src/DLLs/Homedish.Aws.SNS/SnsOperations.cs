using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Homedish.Aws.SQS;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Newtonsoft.Json;

namespace Homedish.Aws.SNS
{
    public class SnsOperations : ISnsOperations
    {
        private readonly ISqsOperations _sqsOperations;
        private readonly ILogger _logger;
        private readonly IAmazonSimpleNotificationService _client = new AmazonSimpleNotificationServiceClient(RegionEndpoint.APSoutheast2);

        public SnsOperations(ISqsOperations sqsOperations, ILogger logger)
        {
            _sqsOperations = sqsOperations;
            _logger = logger;
        }

        public async Task<string> TopicExists(string name)
        {
            var topic = await _client.FindTopicAsync(name);

            _logger.Info(topic == null ? $"Topic {name} do not exists" : $"Topic {name} already exists");

            return topic?.TopicArn;
        }

        public async Task<string> CreateTopic(string name)
        {
            try
            {
                var topic = await _client.CreateTopicAsync(new CreateTopicRequest
                {
                    Name = name
                });

                _logger.Info($"Topic {name} successfully created");

                return topic.TopicArn;
            }
            catch (Exception exception)
            {
                _logger.Error($"Topic {name} could not be created", exception.ToString());

                return null;
            }
        }

        public async Task<bool> LinkTopicToQueue(string topicArn, string queueUrl)
        {
            try
            {
                var sqsClient = _sqsOperations.GetClient();

                var linkResult = await _client.SubscribeQueueAsync(topicArn, sqsClient, queueUrl);
                if (linkResult == null)
                {
                    _logger.Error($"Topic {topicArn} could not be linked to queue {queueUrl}");
                }
                else
                {
                    _logger.Info($"Topic {topicArn} could be linked successfully to queue {queueUrl}");
                }

                return linkResult != null;
            }
            catch (Exception exception)
            {
                _logger.Error($"Topic {topicArn} could not be linked to queue {queueUrl}", exception.ToString());

                return false;
            }
        }

        public async Task<bool> IsTopicLinkedToQueue(string topicArn, string queueUrl)
        {
            try
            {
                var listSubscriptionsByTopicResponse = await _client.ListSubscriptionsByTopicAsync(new ListSubscriptionsByTopicRequest
                {
                    TopicArn = topicArn
                });

                if (listSubscriptionsByTopicResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    _logger.Error($"Could not list the subscriptions for the topic {topicArn}");
                    return false;
                }

                if (listSubscriptionsByTopicResponse.Subscriptions.Any(sub =>
                    string.Compare(sub.Endpoint, queueUrl, StringComparison.Ordinal) == 0))
                {
                    _logger.Info($"The topic {topicArn} is already linked to the queue {queueUrl}");
                    return true;
                }

                _logger.Info($"The topic {topicArn} is already linked to the queue {queueUrl}");
                return false;
            }
            catch (Exception exception)
            {
                _logger.Error($"Could not determine if the topic {topicArn} is linked to the queue {queueUrl}", exception.ToString());
                return false;
            }
        }

        public async Task<bool> Publish<TEvent>(TEvent @event, string topicArn) where TEvent : Event
        {
            try
            {
                var publishResponse = await _client.PublishAsync(new PublishRequest()
                {
                    Message = JsonConvert.SerializeObject(@event),
                    TopicArn = topicArn
                });

                if (publishResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    _logger.Error($"Could not publish to {EventUtils.GetTopicName(@event)}");
                }

                return publishResponse.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception exception)
            {
                _logger.Error($"Could not publish to {EventUtils.GetTopicName(@event)}", exception.ToString());

                return false;
            }
        }
    }
}

