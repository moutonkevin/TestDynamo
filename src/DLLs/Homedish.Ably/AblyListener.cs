using System;
using System.Linq;
using System.Reflection;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Homedish.Ably
{
    public class AblyListener : AblyClient, IListener
    {
        private ILogger _logger { get; }

        public AblyListener(ILogger logger, IConfiguration configuration) : base(configuration)
        {
        }

        private TypeInfo GetHandlerType<TEvent>() where TEvent : Event
        {
            return Assembly.GetEntryAssembly()
                .DefinedTypes
                .Where(t => t.ImplementedInterfaces
                    .Any(inter => inter == typeof(IHandler<TEvent>)))
                .FirstOrDefault();
        }

        private IHandler<TEvent> GetHandler<TEvent>(TypeInfo handlerType) where TEvent : Event
        {
            return (IHandler<TEvent>)Activator.CreateInstance(handlerType);
        }

        private Event GetEvent<TEvent>() where TEvent : Event
        {
            return (Event)Activator.CreateInstance(typeof(TEvent));
        }

        public IListener WithChannel<TEvent>() where TEvent : Event
        {
            var handlerType = GetHandlerType<TEvent>();
            if (handlerType == null)
            {
                _logger.Warn($"No handler that implements {typeof(IHandler<TEvent>)} was found for the event {typeof(TEvent)}");
                return this;
            }

            var handler = GetHandler<TEvent>(handlerType);
            var @event = GetEvent<TEvent>();
            var channel = _ablyRealtime.Channels.Get(@event.ChannelName);

            channel.Subscribe(async (m) =>
            {
                await handler.HandleAsync(JsonConvert.DeserializeObject<TEvent>(m.Data.ToString()));
            });

            return this;
        }

        public void StopListening()
        {
            throw new NotImplementedException();
        }
    }
}
