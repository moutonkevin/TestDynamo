using System;
using System.Threading.Tasks;
using Homedish.Events.Contracts;
using Homedish.Logging;

namespace Homedish.Template.Worker.Handlers
{
    public class TestHandler : IHandler<TestEvent>
    {
        private ILogger Logger { get; }

        public TestHandler(ILogger logger)
        {
            Logger = logger;
        }

        public async Task<bool> HandleAsync(TestEvent message)
        {
            Logger.Info($"{nameof(TestEvent)} received", message);

            Console.WriteLine(message.Content);

            return true;
        }
    }
}
