using IO.Ably;
using Microsoft.Extensions.Configuration;

namespace Homedish.Ably
{
    public class AblyClient
    {
        protected AblyRealtime _ablyRealtime { get; set; }

        public AblyClient(IConfiguration configuration)
        {
            _ablyRealtime = new AblyRealtime(configuration["AblyApiKey"]);
        }
    }
}
