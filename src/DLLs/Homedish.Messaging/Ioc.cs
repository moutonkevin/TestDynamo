using System;
using Homedish.Aws.SNS;
using Homedish.Aws.SQS;
using Homedish.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Homedish.Messaging
{
    public static class Ioc
    {
        private static readonly ServiceCollection services = new ServiceCollection();
        public static IServiceProvider Services { get; set; }

        public static void InjectNonParamaterableServices()
        {
            services
                .AddTransient<ISnsOperations, SnsOperations>()
                .AddTransient<ISqsOperations, SqsOperations>()
                .AddSingleton<IMessagingInitializer, MessagingInitializer>();
        }

        public static void InjectParamaterableServices<TDefinition>(TDefinition implementation)
            where TDefinition : class
        {
            services
                .AddTransient(s => implementation);
        }

        public static void BuildServiceProvider()
        {
            Services = services.BuildServiceProvider();
        }
    }
}
