using Homedish.Logging;
using Homedish.Template.Worker.Core.Repositories;
using Microsoft.Extensions.Configuration;

namespace Homedish.Template.Worker.Core.Services
{
    public class TestService : ITestService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configurations;
        private readonly ITestRepository _testRepository;

        public TestService(ILogger logger, IConfiguration configurations, ITestRepository testRepository)
        {
            _logger = logger;
            _configurations = configurations;
            _testRepository = testRepository;
        }

        public string Get(int id)
        {
            return _testRepository.Get(id);
        }
    }
}
