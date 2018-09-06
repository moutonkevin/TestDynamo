using System.Threading.Tasks;
using Homedish.Aws.SQS;
using Homedish.Events.Contracts;
using Xunit;

namespace Homedish.Messaging.UnitTests
{
    public class Class1
    {
        [Fact]
        public async Task Test()
        {
            IMessagingProcessor messagingProcessor = new MessagingProcessor();

            await messagingProcessor.Send(new TestEvent
            {
                Content = "hello"
            });
        }
    }
}
