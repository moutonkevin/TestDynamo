using Homedish.Ably;
using Homedish.Core.Authentication;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Homedish.Template.Core.Repositories;
using Homedish.Template.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homedish.Template.Api
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
            services.AddCustomLogging();
            services.AddAuthentication(options =>
            {
                options.AddScheme<ApiKeyAuthenticationHandler>("ApiKey", "API Key");
            });

            services.AddTransient<ITestService, TestService>();
            services.AddTransient<ITestRepository, TestRepository>();
            services.AddTransient<IPublisher, AblyPublisher>();
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
