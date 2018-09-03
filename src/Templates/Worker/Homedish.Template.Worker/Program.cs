using Homedish.Ably;
using Homedish.Events.Contracts;
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
                .WithChannel<TestEvent>();

            var publisher = host.Services.GetService<IPublisher>();

            var task = publisher.PublishAsync(new TestEvent
            {
                Content = "hihi"
            });
            task.Wait();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>();
    }
}
