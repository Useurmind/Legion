using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

using Legion.Core.Messages;
using Legion.Core.Messages.Serialization;
using Legion.Kafka;

using Newtonsoft.Json;

namespace Heliu.Core.Kafka
{
    public class KafkaMessageSender : IMessageSender
    {
        private Producer producer;

        private readonly IMessageSerializer messageSerializer;

        public KafkaMessageSender(Producer producer, IMessageSerializer messageSerializer)
        {
            this.producer = producer;
            this.messageSerializer = messageSerializer;
        }

        /// <inheritdoc />
        public async Task SendMessageAsync(string topic, object key, object header, object message)
        {
            var messageBytes = this.messageSerializer.Serialize(message);

            var dr = this.producer.ProduceAsync(topic, null, messageBytes).Result;
            Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
        }
    }
}