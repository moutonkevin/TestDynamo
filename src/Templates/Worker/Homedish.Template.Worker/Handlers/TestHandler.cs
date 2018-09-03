using System;
using System.Threading.Tasks;
using Homedish.Events.Contracts;

namespace Homedish.Template.Worker.Handlers
{
    public class TestHandler : IHandler<TestEvent>
    {
        public async Task<bool> HandleAsync(TestEvent message)
        {
            Console.WriteLine(message.Content);

            return true;
        }
    }
}
