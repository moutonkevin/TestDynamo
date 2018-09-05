using System.Threading.Tasks;
using Homedish.Logging;
using Homedish.Template.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Homedish.Template.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configurations;
        private readonly ITestService _testService;

        public TestController(ILogger logger, IConfiguration configurations, ITestService testService)
        {
            _logger = logger;
            _configurations = configurations;
            _testService = testService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            _logger.Info($"Placeholder for {_configurations["FeatureName"]} and ID = {id}");

            var value = await _testService.Get(id);

            return value;
        }
    }
}
