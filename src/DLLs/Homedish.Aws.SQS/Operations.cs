using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Homedish.Aws.SQS.Models;
using Newtonsoft.Json;

namespace Homedish.Aws.SQS
{
    public class Operations : IOperations
    {
        private static readonly IAmazonSQS Client = new AmazonSQSClient();

        public async Task<bool> CreateQueue(string name, int maxKeepDurationSeconds)
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

            var createQueueResponse = await Client.CreateQueueAsync(createQueueRequest);

            return createQueueResponse.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Enqueue(QueueConfiguration configuration, object data)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = configuration.QueueUrl,
                MessageBody = JsonConvert.SerializeObject(data)
            };

            var sendMessageResponse = await Client.SendMessageAsync(sendMessageRequest);

            return sendMessageResponse.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<IEnumerable<string>> Dequeue(QueueConfiguration configuration)
        {
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = configuration.QueueUrl,
            };

            var receiveMessageResponse = await Client.ReceiveMessageAsync(receiveMessageRequest);

            return receiveMessageResponse.Messages.Select(messages => messages.Body).ToList();
        }
    }
}
