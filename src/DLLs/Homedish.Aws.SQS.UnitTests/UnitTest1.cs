using System;
using System.Threading.Tasks;
using Xunit;

namespace Homedish.Aws.SQS.UniTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            Operations qc = new Operations();

            await qc.CreateQueue("test-sqs-qa53", 43000);
        }
    }
}
