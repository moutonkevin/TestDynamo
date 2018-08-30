using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ILogger = Homedish.Logging.ILogger;

namespace Homedish.WebCore.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configurations;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory loggerFactory, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IConfiguration configurations,
            ILogger logger) : base(options, loggerFactory, encoder, clock)
        {

            _configurations = configurations;
            _logger = logger;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            const string authorizationHeaderKey = "Authorization";

            if (Request?.HttpContext?.Request?.Headers == null)
            {
                _logger.Error($"Could not authenticate the request for {typeof(ApiKeyAuthenticationHandler)}");

                return Task.FromResult(AuthenticateResult.Fail($"Could not authenticate the request for {typeof(ApiKeyAuthenticationHandler)}"));
            }

            if (!Request.HttpContext.Request.Headers.ContainsKey(authorizationHeaderKey))
            {
                _logger.Error("No Authorization header was provided for the request");

                return Task.FromResult(AuthenticateResult.Fail("No Authorization header was provided for the request"));
            }

            var clientAuthorizationHeader = Request.HttpContext.Request.Headers[authorizationHeaderKey];
            var serverApiKey = _configurations["ApiKey"];

            if (clientAuthorizationHeader.Equals(serverApiKey))
            {
                var claims = new[] { new Claim(ClaimTypes.Authentication, "ApiKey") };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            _logger.Error("The Authorization key is invalid");

            return Task.FromResult(AuthenticateResult.Fail("The Authorization key is invalid"));
        }
    }
}
