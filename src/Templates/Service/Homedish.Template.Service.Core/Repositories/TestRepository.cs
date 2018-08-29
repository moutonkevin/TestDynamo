using Homedish.Logging;
using Microsoft.Extensions.Configuration;

namespace Homedish.Template.Service.Core.Repositories
{
    public class TestRepository
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configurations;

        public TestRepository(ILogger logger, IConfiguration configurations)
        {
            _logger = logger;
            _configurations = configurations;
        }
    }
}
