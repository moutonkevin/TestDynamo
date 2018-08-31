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

        public TestService(ILogger logger, IConfiguration configurations, ITestRepository repository)
        {
            _logger = logger;
            _configurations = configurations;
            _repository = repository;
        }

        public string Get(int id)
        {
            var value = _repository.Get(id);

            return value;
        }
    }
}
