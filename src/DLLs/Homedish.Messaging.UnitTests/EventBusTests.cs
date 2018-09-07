using System;
using System.Threading;
using System.Threading.Tasks;
using Homedish.Aws.SNS;
using Homedish.Aws.SQS;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Homedish.Messaging.UnitTests
{
    public class EventBusTests
    {
        private ServiceCollection Services { get; } = new ServiceCollection();

        public EventBusTests()
        {
            Services
                .AddTransient<ILogger, Logger>();
        }

        [Fact]
        public async Task PublishAsync_ReturnsTrue()
        {
            Services
                .AddTransient<ILogger, Logger>()
                .AddTransient<ISnsOperations, SnsOperations>()
                .AddTransient<ISqsOperations, SqsOperations>()
                .AddTransient<IPublisher, EventBus>()
                .AddSingleton<IEventBusInitializer, EventBusInitializer>();

            var serviceProvider = Services.BuildServiceProvider();

            var messagingProcessor = serviceProvider.GetService<IPublisher>();

            var isSuccess = await messagingProcessor.PublishAsync(new TestEvent
            {
                Content = "hello"
            });

            Assert.True(isSuccess);
        }

        [Fact]
        public async Task ListenTo_AtLeastOneEventPulledFromQueue()
        {
            //Setup

            var handlerMock = new Mock<IHandler<TestEvent>>();

            Services
                .AddTransient<ILogger, Logger>()
                .AddTransient<ISnsOperations, SnsOperations>()
                .AddTransient<ISqsOperations, SqsOperations>()
                .AddSingleton<IEventBusInitializer, EventBusInitializer>()
                .AddSingleton<IHandler<TestEvent>>(handlerMock.Object)
                .AddTransient<IListener, EventBus>()
                .AddTransient<IPublisher, EventBus>();

            var serviceProvider = Services.BuildServiceProvider();

            var listener = serviceProvider.GetService<IListener>();
            var publisher = serviceProvider.GetService<IPublisher>();

            //Act

            var isPublished = await publisher.PublishAsync(new TestEvent
            {
                Content = "test"
            });

            listener.ListenTo<TestEvent>();

            Thread.Sleep(1000);

            //Assert
            Assert.True(isPublished);
            handlerMock.Verify(s => s.HandleAsync(It.IsAny<TestEvent>()), Times.AtLeastOnce);
        }
    }
}
