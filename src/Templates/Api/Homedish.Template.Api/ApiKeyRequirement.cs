using System.Threading.Tasks;
using Homedish.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Homedish.Template.Api
{
    public class ApiKeyRequirement : AuthorizationHandler<ApiKeyRequirement>, IAuthorizationRequirement
    {
        private const string AuthorizationHeaderKey = "Authorization";

        private readonly ILogger _logger;
        private readonly IConfiguration _configurations;

        public ApiKeyRequirement(ILogger logger, IConfiguration configurations)
        {
            _logger = logger;
            _configurations = configurations;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            var requestContext = (Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext) context.Resource;

            if (requestContext?.HttpContext?.Request?.Headers == null)
            {
                _logger.Error($"Could not authenticate the request for {typeof(ApiKeyRequirement)}");
                context.Fail();

                return Task.CompletedTask;
            }

            if (!requestContext.HttpContext.Request.Headers.ContainsKey(AuthorizationHeaderKey))
            {
                _logger.Error("No Authorization header was provided for the request");
                context.Fail();

                return Task.CompletedTask;
            }

            var clientAuthorizationHeader = requestContext.HttpContext.Request.Headers[AuthorizationHeaderKey];
            var serverApiKey = _configurations["ApiKey"];

            if (clientAuthorizationHeader.Equals(serverApiKey))
            {
                context.Succeed(requirement);
            }
            else
            {
                _logger.Error("The Authorization key is invalid");
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
