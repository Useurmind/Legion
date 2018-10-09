using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Confluent.Kafka;

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

        private readonly Consumer<byte[], byte[]> consumer;

        private readonly IDictionary<string, long> consumeFromOffset;

        private readonly IMessageSerializer messageSerializer;

        public KafkaMessageListener(
            Consumer<byte[], byte[]> consumer,
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
                var result = this.consumer.Consume(TimeSpan.FromMilliseconds(100));
                if(result == null)
                {
                    continue;
                }

                Console.WriteLine($"Topic: {result.Topic} Partition: {result.Partition} Offset: {result.Offset} {result.Value}");

                await this.HandleMessage(result);

                Console.WriteLine($"Committing offset");
                var committedOffsets = this.consumer.Commit(result);
                Console.WriteLine($"Committed offset: {committedOffsets}");
            }
        }

        private async Task HandleMessage(ConsumeResult<byte[], byte[]> message)
        {
            var messageObject = this.messageSerializer.Deserialize(message.Value);

            await this.handleMessageObject(null, null, messageObject);
        }
    }
}