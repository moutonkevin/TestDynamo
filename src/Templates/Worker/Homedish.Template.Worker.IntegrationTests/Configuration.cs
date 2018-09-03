using Microsoft.Extensions.Configuration;

namespace Homedish.Template.Worker.IntegrationTests
{
    public class Configuration
    {
        public static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
        }
    }
}
