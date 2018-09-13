using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Runtime.SharedInterfaces;
using Amazon.SQS.Model;
using Homedish.Aws.SQS.Models;
using Homedish.Events.Contracts;

namespace Homedish.Aws.SQS
{
    public interface ISqsOperations
    {
        ICoreAmazonSQS GetClient();
        Task<bool> QueueExists(string name);
        Task<string> GetQueueUrl(string name);
        Task<string> CreateQueue(string name, EventConfiguration configuration);
        Task<bool> Enqueue(QueueConfiguration configuration, object data);
        Task<IEnumerable<Message>> Dequeue(QueueConfiguration configuration);
        Task<IEnumerable<Message>> Dequeue(string queueUrl);
        Task Delete(string queueUrl, Message message);
    }
}
