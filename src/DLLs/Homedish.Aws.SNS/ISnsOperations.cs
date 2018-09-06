using System.Threading.Tasks;
using Homedish.Events.Contracts;

namespace Homedish.Aws.SNS
{
    public interface ISnsOperations
    {
        Task<string> TopicExists(string name);
        Task<string> CreateTopic(string name);
        Task<bool> LinkTopicToQueue(string topicArn, string queueUrl);
        Task<bool> IsTopicLinkedToQueue(string topicArn, string queueUrl);
        Task<bool> Publish<TEvent>(TEvent @event, string topicArn) where TEvent : Event;
    }
}
