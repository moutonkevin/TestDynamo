using Homedish.Ably;
using Homedish.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homedish.Template.Worker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IListener, Listener>();
            services.AddTransient<ILogger, Logger>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           
        }
    }
}
