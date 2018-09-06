namespace Homedish.Events.Contracts
{
    public class EventUtils
    {
        private static readonly object Lock = new object();

        public static string GetTopicName<TEvent>(TEvent @event) where TEvent : Event
        {
            lock (Lock)
            {
                return $"sns-{@event.ChannelName.ToLowerInvariant()}";
            }
        }

        public static string GetQueueName<TEvent>(TEvent @event) where TEvent : Event
        {
            lock (Lock)
            {
                return $"sqs-{@event.ChannelName.ToLowerInvariant()}";
            }
        }
    }
}
