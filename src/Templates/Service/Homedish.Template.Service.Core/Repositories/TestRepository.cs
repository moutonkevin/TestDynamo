using Homedish.Logging;
using Microsoft.Extensions.Configuration;

namespace Homedish.Template.Worker.Core.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configurations;

        public TestRepository(ILogger logger, IConfiguration configurations)
        {
            _logger = logger;
            _configurations = configurations;
        }

        public string Get(int id)
        {
            return "hello";
        }
    }
}
