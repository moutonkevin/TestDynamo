using Homedish.Ably;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Homedish.Template.Worker.Core.Repositories;
using Homedish.Template.Worker.Core.Services;
using Homedish.Template.Worker.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<IListener, AblyListener>();
            services.AddTransient<IPublisher, AblyPublisher>();
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<ITestRepository, TestRepository>();
            services.AddTransient<ILogger, Logger>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
