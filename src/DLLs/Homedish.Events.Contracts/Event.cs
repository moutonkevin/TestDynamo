using System;

namespace Homedish.Events.Contracts
{
    public abstract class Event
    {
        public abstract string ChannelName { get; }
        public DateTime Creation { get; } = DateTime.UtcNow;
    }
}
