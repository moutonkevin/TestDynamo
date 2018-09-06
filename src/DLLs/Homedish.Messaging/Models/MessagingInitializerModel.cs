using System;

namespace Homedish.Messaging.Models
{
    public class MessagingInitializerModel
    {
        public Type EventType { get; set; }
        public string TopicArn { get; set; }
        public bool IsSuccessfullyInitialized { get; set; } = false;
    }
}
