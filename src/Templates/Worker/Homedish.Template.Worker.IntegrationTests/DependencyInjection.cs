using System;
using Homedish.Ably;
using Homedish.Events.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homedish.Template.Worker.IntegrationTests
{
    public class DependencyInjection : IDisposable
    {
        public IServiceProvider ServiceProvider { get; set; }

        public DependencyInjection()
        {
            ServiceProvider = new ServiceCollection()
                .AddScoped<IPublisher, AblyPublisher>()
                .AddScoped<IConfiguration>(p => Configuration.GetIConfigurationRoot())
                .BuildServiceProvider();
        }

        public void Dispose()
        {
        }
    }
}
