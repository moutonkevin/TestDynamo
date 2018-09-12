using Homedish.Events.Contracts;
using Homedish.Template.Worker.Handlers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Homedish.Template.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            var listener = host.Services.GetService<IListener>();

            listener
                .StartListening<TestEvent, TestHandler>();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>();
    }
}
