using Homedish.Events.Contracts;
using Homedish.Template.Worker.Handlers;
using Microsoft.Extensions.Configuration;

namespace Homedish.Template.Worker.Messaging
{
    public class MessagingStartup : IMessagingStartup
    {
        private readonly IListener _listener;
        private readonly IConfiguration _configuration;

        public MessagingStartup(IListener listener, IConfiguration configuration)
        {
            _listener = listener;
            _configuration = configuration;
        }

        public void Initialize()
        {
            _listener
                .StartListening<TestEvent, TestHandler>();
        }
    }
}
