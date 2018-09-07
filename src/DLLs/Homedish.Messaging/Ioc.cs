using System;
using System.Linq;
using Homedish.Aws.SNS;
using Homedish.Aws.SQS;
using Homedish.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Homedish.Messaging
{
    public static class Ioc
    {
        public static readonly ServiceCollection services = new ServiceCollection();
        public static IServiceProvider Services { get; set; }

        public static void InjectNonParamaterableServices()
        {
            if (Services == null)
            {
                services
                    .AddTransient<ISnsOperations, SnsOperations>()
                    .AddTransient<ISqsOperations, SqsOperations>()
                    .AddSingleton<IEventBusInitializer, EventBusInitializer>();
            }
        }

        public static void InjectParamaterableServices<TDefinition>(TDefinition implementation)
            where TDefinition : class
        {
            if (Services == null)
            {
                services
                    .AddTransient(s => implementation);
            }
        }

        public static void BuildServiceProvider()
        {
            if (Services == null)
            {
                Services = services.BuildServiceProvider();
            }
        }
    }
}
