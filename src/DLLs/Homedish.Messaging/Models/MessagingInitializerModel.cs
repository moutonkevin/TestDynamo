using System;
using System.Threading;

namespace Homedish.Messaging.Models
{
    public class MessagingInitializerModel
    {
        public Type EventType { get; set; }
        public string TopicArn { get; set; }
        public string QueueUrl { get; set; }
        public bool IsSuccessfullyInitialized { get; set; } = false;
        public CancellationTokenSource CancellationToken { get; set; }
    }
}
