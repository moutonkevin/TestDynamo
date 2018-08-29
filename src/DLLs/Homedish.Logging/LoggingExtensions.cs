using System;
using Microsoft.Extensions.DependencyInjection;

namespace Homedish.Logging
{
    public static class LoggingExtensions
    {
        public static void AddCustomLogging(this IServiceCollection services)
        {
            services.AddSingleton((s) =>
            {
                new LoggingConfiguration().ConfigureWithFileTarget();
                return LogManager.GetLogger();
            });
        }

        public static void AddCustomLogging(this IServiceCollection services, Action setup)
        {
            services.AddSingleton((s) => LogManager.GetLogger());

            setup.Invoke();
        }
    }
}
