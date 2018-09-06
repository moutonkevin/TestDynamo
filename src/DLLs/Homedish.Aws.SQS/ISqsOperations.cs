using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Runtime.SharedInterfaces;
using Homedish.Aws.SQS.Models;

namespace Homedish.Aws.SQS
{
    public interface ISqsOperations
    {
        ICoreAmazonSQS GetClient();
        Task<bool> QueueExists(string name);
        Task<string> GetQueueUrl(string name);
        Task<string> CreateQueue(string name, int maxKeepDurationSeconds);
        Task<bool> Enqueue(QueueConfiguration configuration, object data);
        Task<IEnumerable<string>> Dequeue(QueueConfiguration configuration);
    }
}
