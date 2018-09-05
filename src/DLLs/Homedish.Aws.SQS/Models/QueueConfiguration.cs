namespace Homedish.Aws.SQS.Models
{
    public class QueueConfiguration
    {
        public string AwsRegion { get; set; }
        public string QueueName { get; set; }
        public int AwsAccountNumber { get; set; }
        public string QueueUrl => $"https://sqs.{AwsRegion}.amazonaws.com/{AwsAccountNumber}/{QueueName}";
    }
}
