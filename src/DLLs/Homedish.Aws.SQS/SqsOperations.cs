﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime.SharedInterfaces;
using Amazon.SQS;
using Amazon.SQS.Model;
using Homedish.Aws.SQS.Models;
using Homedish.Events.Contracts;
using Homedish.Logging;
using Newtonsoft.Json;

namespace Homedish.Aws.SQS
{
    public class SqsOperations : ISqsOperations
    {
        private readonly ILogger _logger;
        private readonly IAmazonSQS _client = new AmazonSQSClient(RegionEndpoint.APSoutheast2);

        public SqsOperations(ILogger logger)
        {
            _logger = logger;
        }

        public ICoreAmazonSQS GetClient()
        {
            return _client;
        }

        public async Task<bool> QueueExists(string name)
        {
            try
            {
                await _client.GetQueueUrlAsync(name);

                _logger.Info($"Queue {name} exists");

                return true;
            }
            catch (Exception)
            {
                _logger.Info($"Queue {name} do not exist");

                return false;
            }
        }

        public async Task<string> GetQueueUrl(string name)
        {
            try
            {
                var queueUrlResponse = await _client.GetQueueUrlAsync(name);

                if (queueUrlResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    _logger.Error($"Could not get the queue url for {name}", queueUrlResponse.HttpStatusCode);
                    return null;
                }

               return queueUrlResponse.QueueUrl;
            }
            catch (Exception e)
            {
                _logger.Error($"Could not get the queue url for {name}", e.ToString());

                return null;
            }
        }

        public async Task<string> CreateQueue(string name, EventConfiguration configuration)
        {
            var createQueueRequest = new CreateQueueRequest
            {
                QueueName = name,
                Attributes =
                {
                    { "ReceiveMessageWaitTimeSeconds",  $"{configuration.ReceiveMessageWaitTimeSeconds}"},
                    { "MessageRetentionPeriod",  $"{configuration.MessageRetentionPeriodSeconds}"}
                }
            };

            var createQueueResponse = await _client.CreateQueueAsync(createQueueRequest);
            if (createQueueResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                _logger.Error($"Could not create queue {name}", createQueueResponse.HttpStatusCode);
            }

            return createQueueResponse.QueueUrl;
        }

        public async Task<bool> Enqueue(QueueConfiguration configuration, object data)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = configuration.QueueUrl,
                MessageBody = JsonConvert.SerializeObject(data)
            };

            var sendMessageResponse = await _client.SendMessageAsync(sendMessageRequest);
            if (sendMessageResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                _logger.Error($"Could not enqueue in {configuration.QueueUrl}", sendMessageResponse.HttpStatusCode);
            }

            return sendMessageResponse.HttpStatusCode == HttpStatusCode.OK;
        }

        private async Task<IEnumerable<Message>> DequeueInternal(string queueUrl)
        {
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
            };

            try
            {
                var receiveMessageResponse = await _client.ReceiveMessageAsync(receiveMessageRequest);
                if (receiveMessageResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    _logger.Error($"Could not dequeue in {queueUrl}", receiveMessageResponse.HttpStatusCode);
                }

                return receiveMessageResponse.Messages.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Message>> Dequeue(QueueConfiguration configuration)
        {
            return await DequeueInternal(configuration.QueueUrl);
        }

        public async Task<IEnumerable<Message>> Dequeue(string queueUrl)
        {
            return await DequeueInternal(queueUrl);
        }

        public async Task Delete(string queueUrl, Message message)
        {
            await _client.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = message.ReceiptHandle
            });
        }
    }
}
