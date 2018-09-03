using Homedish.Events.Contracts;
using IO.Ably;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Homedish.Ably
{
    public class AblyPublisher : AblyClient, IPublisher
    {
        public AblyPublisher(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<bool> PublishAsync<TEvent>(TEvent content) where TEvent : Event
        {
            var channel = _ablyRealtime.Channels.Get(content.ChannelName);

            var result = await channel.PublishAsync(new Message
            {
                Name = "name",
                Data = content
            });

            return result.IsSuccess;
        }
    }
}
