using System.Threading.Tasks;
using Homedish.Events.Contracts;

namespace Homedish.Messaging.UnitTests.Handlers
{
    public class TestEventHandler : IHandler<TestEvent>
    {
        public static bool HasBeenCalled { get; set; }

        public Task<bool> HandleAsync(TestEvent message)
        {
            HasBeenCalled = true;

            return Task.FromResult(true);
        }
    }
}
