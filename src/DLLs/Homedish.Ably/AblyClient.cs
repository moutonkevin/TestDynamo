using System;
using System.Security;
using Homedish.WebCore.Cryptography;
using IO.Ably;
using Microsoft.Extensions.Configuration;

namespace Homedish.Ably
{
    public class AblyClient
    {
        protected AblyRealtime AblyRealtime { get; set; }

        public AblyClient(IConfiguration configuration)
        {
            const string ablyApiKeyName = "Ably:ApiKey";

            var apiKey = configuration[ablyApiKeyName];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException($"The {ablyApiKeyName} key should be specified in the configs");
            }

            AblyRealtime = new AblyRealtime(apiKey);
        }
    }
}
