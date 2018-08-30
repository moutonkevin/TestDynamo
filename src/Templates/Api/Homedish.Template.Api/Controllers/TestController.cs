using Homedish.Logging;
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

        public TestController(ILogger logger, IConfiguration configurations)
        {
            _logger = logger;
            _configurations = configurations;
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            _logger.Info($"Placeholder for {_configurations["FeatureName"]} and ID = {id}");

            return string.Empty;
        }
    }
}
