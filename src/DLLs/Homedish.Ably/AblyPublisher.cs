using System;
using Homedish.Events.Contracts;
using IO.Ably;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Homedish.Logging;

namespace Homedish.Ably
{
    public class AblyPublisher : AblyClient, IPublisher
    {
        private readonly ILogger _logger;

        public AblyPublisher(IConfiguration configuration, ILogger logger) : base(configuration)
        {
            _logger = logger;
        }

        public async Task<bool> PublishAsync<TEvent>(TEvent content, EventConfiguration configuration = null) where TEvent : Event
        {
            var channel = AblyRealtime.Channels.Get(content.ChannelName);

            _logger.Info($"Publishing event {typeof(TEvent)}", content);

            try
            {
                var result = await channel.PublishAsync(new Message
                {
                    Data = content
                });

                if (result.IsFailure)
                {
                    _logger.Error($"Failed to publish event {typeof(TEvent)}", nameof(TEvent), content, result.Error.Message);
                }

                return result.IsSuccess;
            }
            catch (Exception exception)
            {
                _logger.Error($"Failed to publish event {typeof(TEvent)}", nameof(TEvent), content, exception.ToString());
                return false;
            }
        }
    }
}
