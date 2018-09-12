using System.Threading;
using System.Threading.Tasks;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Homedish.Messaging.UnitTests.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Homedish.Messaging.UnitTests
{
    public class EventBusTests
    {
        private IServiceCollection Services { get; } = new ServiceCollection();

        public EventBusTests()
        {
        }

        [Fact]
        public async Task PublishAsync_ReturnsTrue()
        {
            //Setup
            Services
                .AddTransient<IPublisher, EventBus>()
                .AddCustomLogging();

            var serviceProvider = Services.BuildServiceProvider();

            var messagingProcessor = serviceProvider.GetService<IPublisher>();

            //Act
            var isSuccess = await messagingProcessor.PublishAsync(new TestEvent
            {
                Content = "hello"
            });

            //Assert
            Assert.True(isSuccess);
        }

        [Fact]
        public async Task StartListening_AtLeastOneEventPulledFromQueue()
        {
            //Setup
            Services
                .AddTransient<IListener, EventBus>()
                .AddTransient<IPublisher, EventBus>()
                .AddCustomLogging();

            var serviceProvider = Services.BuildServiceProvider();

            var listener = serviceProvider.GetService<IListener>();
            var publisher = serviceProvider.GetService<IPublisher>();

            //Act
            var isPublished = await publisher.PublishAsync(new TestEvent
            {
                Content = "test"
            });

            Thread.Sleep(1000);

            listener.StartListening<TestEvent, TestEventHandler>();

            Thread.Sleep(1000);

            //Assert
            Assert.True(isPublished);
            Assert.True(TestEventHandler.HasBeenCalled);
        }

        [Fact]
        public async Task StartListening_AllEventsPulledFromQueue()
        {
            //Setup
            Services
                .AddTransient<IListener, EventBus>()
                .AddTransient<IPublisher, EventBus>()
                .AddCustomLogging();

            var serviceProvider = Services.BuildServiceProvider();

            var listener = serviceProvider.GetService<IListener>();
            var publisher = serviceProvider.GetService<IPublisher>();

            //Act
            await publisher.PublishAsync(new TestEvent { Content = "test" });

            Thread.Sleep(1000);

            listener.StartListening<TestEvent, TestEventHandler>();

            Thread.Sleep(1000);

            //Assert
            Assert.True(TestEventHandler.HasBeenCalled);

            //Setup
            TestEventHandler.HasBeenCalled = false;

            //Act
            await publisher.PublishAsync(new TestEvent { Content = "test" });

            Thread.Sleep(25000);

            //Assert
            Assert.True(TestEventHandler.HasBeenCalled);
        }
    }
}
