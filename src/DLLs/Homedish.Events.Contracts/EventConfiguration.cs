namespace Homedish.Events.Contracts
{
    public class EventConfiguration
    {
        public int ReceiveMessageWaitTimeSeconds { get; set; }
        public int MessageRetentionPeriodSeconds { get; set; }
    }

    public class DefaultEventConfiguration : EventConfiguration
    {
        public DefaultEventConfiguration()
        {
            ReceiveMessageWaitTimeSeconds = 20; //20sec
            MessageRetentionPeriodSeconds = 1209600; // 14days
        }
    }
}
