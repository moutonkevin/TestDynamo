using System;

namespace Homedish.Events.Contracts
{
    public class TestEvent : Event
    {
        public override string ChannelName { get; set; } = "test";
        public string Content { get; set; }
    }
}
