using System.Collections.Generic;
using System.Threading.Tasks;
using Homedish.Aws.SQS.Models;

namespace Homedish.Aws.SQS
{
    public interface IOperations
    {
        Task<bool> CreateQueue(string name, int maxKeepDurationSeconds);
        Task<bool> Enqueue(QueueConfiguration configuration, object data);
        Task<IEnumerable<string>> Dequeue(QueueConfiguration configuration);
    }
}
