namespace Homedish.Events.Contracts
{
    public class TestEvent : Event
    {
        public override string ChannelName { get; } = "test";
        public string Content { get; set; }
    }
}
