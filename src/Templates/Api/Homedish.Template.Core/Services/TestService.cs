using System.Threading.Tasks;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Homedish.Template.Core.Repositories;
using Microsoft.Extensions.Configuration;

namespace Homedish.Template.Core.Services
{
    public class TestService : ITestService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configurations;
        private readonly ITestRepository _repository;
        private readonly IPublisher _publisher;

        public TestService(ILogger logger, IConfiguration configurations, ITestRepository repository, IPublisher publisher)
        {
            _logger = logger;
            _configurations = configurations;
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<string> Get(int id)
        {
            var value = _repository.Get(id);

            var result = await _publisher.PublishAsync(new TestEvent
            {
                Content = "helloooo"
            });

            return value;
        }
    }
}
