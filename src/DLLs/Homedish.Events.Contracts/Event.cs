using System;

namespace Homedish.Events.Contracts
{
    public abstract class Event
    {
        public abstract string ChannelName { get; set; }
        public DateTime Creation { get; set; }
    }
}
