using System.Threading.Tasks;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Homedish.Template.Worker.Core.Services;

namespace Homedish.Template.Worker.Handlers
{
    public class TestHandler : IHandler<TestEvent>
    {
        private readonly ITestService _service;
        private readonly ILogger _logger;

        public TestHandler(ILogger logger, ITestService service)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<bool> HandleAsync(TestEvent message)
        {
            _logger.Info($"Reiceived {nameof(TestEvent)}", message, nameof(TestEvent));

            //example
            _service.Get(message.GetHashCode());

            return true;
        }
    }
}
