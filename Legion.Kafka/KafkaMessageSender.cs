using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;

using Legion.Core.Messages;
using Legion.Core.Messages.Serialization;
using Legion.Kafka;

using Newtonsoft.Json;

namespace Heliu.Core.Kafka
{
    public class KafkaMessageSender : IMessageSender
    {
        private Producer<byte[], byte[]> producer;

        private readonly IMessageSerializer messageSerializer;

        public KafkaMessageSender(Producer<byte[], byte[]> producer, IMessageSerializer messageSerializer)
        {
            this.producer = producer;
            this.messageSerializer = messageSerializer;
        }

        /// <inheritdoc />
        public async Task SendMessageAsync(string topic, object message, byte[] key = null, IDictionary<string, byte[]> headers = null)
        {
            var serializedMessage = this.messageSerializer.Serialize(message);
            var kafkaMessage = new Message<byte[], byte[]>();
            kafkaMessage.Key = serializedMessage.Key;
            kafkaMessage.Value = serializedMessage.Value;
            foreach (var headerKv in serializedMessage.Headers)
            {
                kafkaMessage.Headers.Add(headerKv.Key, headerKv.Value);
            }

            if (key != null)
            {
                kafkaMessage.Key = key;
            }

            if (headers != null)
            {
                foreach (var headerKv in headers)
                {
                    kafkaMessage.Headers.Add(headerKv.Key, headerKv.Value);
                }

            }

            var dr = this.producer.ProduceAsync(topic, kafkaMessage).Result;
            Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
        }
    }
}