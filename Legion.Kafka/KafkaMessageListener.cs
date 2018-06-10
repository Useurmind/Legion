using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Confluent.Kafka;
using Confluent.Kafka.Serialization;

using Legion.Core.Messages;
using Legion.Core.Messages.Serialization;
using Legion.Core.Messages.Types;
using Legion.Core.Threading;

using Newtonsoft.Json.Linq;

namespace Legion.Kafka
{
    public class KafkaMessageListener : IMessageListener
    {
        private readonly string listenerGroupId;

        private HandleMessageAsync handleMessageObject;

        private readonly Consumer consumer;

        private readonly IDictionary<string, long> consumeFromOffset;

        private readonly IMessageSerializer messageSerializer;

        public KafkaMessageListener(
            Consumer consumer,
            string listenerGroupId,
            IDictionary<string, long> consumeFromOffset,
            IMessageSerializer messageSerializer)
        {
            this.consumer = consumer;
            this.consumeFromOffset = consumeFromOffset;
            this.messageSerializer = messageSerializer;
            this.listenerGroupId = listenerGroupId;
        }

        public async Task StartAsync(
            IEnumerable<string> topics,
            HandleMessageAsync handleMessageObject,
            CancellationToken? cancellationToken = null)
        {
            this.handleMessageObject = handleMessageObject;

            Console.WriteLine("Listening to Kafka messages");

            this.consumer.OnError += (_, error) => Console.WriteLine($"Error: {error}");

            this.consumer.OnConsumeError += (_, msg) =>
                Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");

            if (this.consumeFromOffset != null)
            {
                var topicPartitionOffsets = this.consumeFromOffset.Select(x => new TopicPartitionOffset(
                    new TopicPartition(x.Key, 0),
                    new Offset(x.Value)));
                this.consumer.Assign(topicPartitionOffsets);
            }

            var topicsWithoutOffsets = topics.Except(this.consumeFromOffset.Keys);
            this.consumer.Subscribe(topicsWithoutOffsets);

            while (cancellationToken.KeepRunning())
            {
                Message msg;
                if (!this.consumer.Consume(out msg, TimeSpan.FromMilliseconds(100)))
                {
                    continue;
                }

                Console.WriteLine($"Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {msg.Value}");

                await this.HandleMessage(msg);

                Console.WriteLine($"Committing offset");
                var committedOffsets = this.consumer.CommitAsync(msg).Result;
                Console.WriteLine($"Committed offset: {committedOffsets}");
            }
        }

        private async Task HandleMessage(Message message)
        {
            var messageObject = this.messageSerializer.Deserialize(message.Value);

            await this.handleMessageObject(null, null, messageObject);
        }
    }
}