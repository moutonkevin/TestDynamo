using Homedish.Logging;
using Microsoft.Extensions.Configuration;

namespace Homedish.Template.Service.Core.Services
{
    public class TestService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configurations;

        public TestService(ILogger logger, IConfiguration configurations)
        {
            _logger = logger;
            _configurations = configurations;
        }
    }
}
