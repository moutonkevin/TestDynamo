﻿using System.Threading.Tasks;

namespace Homedish.Messaging
{
    public interface ISnsOperations
    {
        Task<string> TopicExists(string name);
        Task<string> CreateTopic(string name);
        Task<bool> LinkTopicToQueue(string topicArn, string queueUrl);
    }
}
