using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Homedish.Aws.SQS
{
    public interface IOperations
    {
        Task<bool> CreateQueue(string name, int maxKeepDurationSeconds);
    }
}
