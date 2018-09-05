using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Homedish.Aws.SQS
{
    public class Operations : IOperations
    {

        public async Task<bool> CreateQueue(string name, int maxKeepDurationSeconds)
        {
            var createQueueRequest = new CreateQueueRequest
            {
                QueueName = name,
                Attributes =
                {
                    { "VisibilityTimeout",  $"{maxKeepDurationSeconds}"}
                }
            };
            return true;
            //var createQueueResponse = await Client.CreateQueueAsync(createQueueRequest);

            //return createQueueResponse.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
