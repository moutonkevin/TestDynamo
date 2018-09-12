using System;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Homedish.Ably
{
    public class AblyListener : AblyClient, IListener
    {
        private ILogger Logger { get; }
        private IServiceProvider ServiceProvider { get; }

        public AblyListener(ILogger logger, IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
        }

        private IHandler<TEvent> GetHandler<TEvent>() where TEvent : Event
        {
            return Activator.CreateInstance<IHandler<TEvent>>();
        }

        private Event GetEvent<TEvent>() where TEvent : Event
        {
            return (Event)Activator.CreateInstance(typeof(TEvent));
        }

        public void StopListening()
        {
            throw new NotImplementedException();
        }

        public IListener StartListening<TEvent, THandler>() 
            where TEvent : Event 
            where THandler : IHandler<TEvent>
        {
            var handler = GetHandler<TEvent>();
            if (handler == default(IHandler<TEvent>))
            {
                Logger.Warn($"No handler that implements {typeof(IHandler<TEvent>)} was found for the event {typeof(TEvent)}");
                return this;
            }

            var @event = GetEvent<TEvent>();
            var channel = AblyRealtime.Channels.Get(@event.ChannelName);

            channel.Subscribe(async (m) =>
            {
                await handler.HandleAsync(JsonConvert.DeserializeObject<TEvent>(m.Data.ToString()));
            });

            return this;
        }

        public void StopListening<TEvent>() where TEvent : Event
        {
            var @event = GetEvent<TEvent>();
            var channel = AblyRealtime.Channels.Get(@event.ChannelName);

            channel.Unsubscribe();
        }
    }
}
