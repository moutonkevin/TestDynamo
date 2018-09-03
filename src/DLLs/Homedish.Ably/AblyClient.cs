using IO.Ably;
using Microsoft.Extensions.Configuration;

namespace Homedish.Ably
{
    public class AblyClient
    {
        protected AblyRealtime AblyRealtime { get; set; }

        public AblyClient(IConfiguration configuration)
        {
            AblyRealtime = new AblyRealtime(configuration["AblyApiKey"]);
        }
    }
}
