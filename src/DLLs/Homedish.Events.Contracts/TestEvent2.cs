namespace Homedish.Events.Contracts
{
    public class TestEvent2 : Event
    {
        public override string ChannelName { get; set; } = "test2";
        public string Content { get; set; }
    }
}
