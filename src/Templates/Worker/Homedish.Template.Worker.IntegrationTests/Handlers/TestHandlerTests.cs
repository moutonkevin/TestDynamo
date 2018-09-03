using System.Threading.Tasks;
using Xunit;
using Homedish.Events.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Homedish.Template.Worker.IntegrationTests.Handlers
{
    public class TestHandlerTests : TestBase
    {
        public TestHandlerTests(DependencyInjection container) : base(container)
        {
        }

        [Fact]
        public async Task TestEventIsPublished_ReceivedOk()
        {
            var publisher = Container.ServiceProvider.GetService<IPublisher>();

            var isSuccess = await publisher.PublishAsync(new TestEvent
            {
                Content = "test"
            });

            Assert.True(isSuccess);
        }
    }
}
