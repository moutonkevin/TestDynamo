using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Runtime.SharedInterfaces;
using Amazon.SQS;
using Amazon.SQS.Model;
using Homedish.Aws.SQS.Models;
using Newtonsoft.Json;

namespace Homedish.Aws.SQS
{
    public class SqsOperations : ISqsOperations
    {
        private readonly IAmazonSQS _client = new AmazonSQSClient();

        public ICoreAmazonSQS GetClient()
        {
            return _client;
        }

        public async Task<bool> QueueExists(string name)
        {
            try
            {
                await _client.GetQueueUrlAsync(name);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> GetQueueArn(string name)
        {
            var queueUrl = await _client.GetQueueUrlAsync(name);
            var attributes = await _client.GetAttributesAsync(queueUrl.QueueUrl);

            return attributes["QueueArn"];
        }

        public async Task<string> CreateQueue(string name, int maxKeepDurationSeconds)
        {
            var createQueueRequest = new CreateQueueRequest
            {
                QueueName = name,
                Attributes =
                {
                    { "ReceiveMessageWaitTimeSeconds",  "20"}, //long polling
                    { "VisibilityTimeout",  $"{maxKeepDurationSeconds}"}
                }
            };

            var createQueueResponse = await _client.CreateQueueAsync(createQueueRequest);

            return createQueueResponse.QueueUrl;
        }

        public async Task<bool> Enqueue(QueueConfiguration configuration, object data)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = configuration.QueueUrl,
                MessageBody = JsonConvert.SerializeObject(data)
            };

            var sendMessageResponse = await _client.SendMessageAsync(sendMessageRequest);

            return sendMessageResponse.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<IEnumerable<string>> Dequeue(QueueConfiguration configuration)
        {
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = configuration.QueueUrl,
            };

            var receiveMessageResponse = await _client.ReceiveMessageAsync(receiveMessageRequest);

            return receiveMessageResponse.Messages.Select(messages => messages.Body).ToList();
        }
    }
}
