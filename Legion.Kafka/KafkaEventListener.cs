using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

using Legion.Core.Messages;
using Legion.Core.Messages.Types;
using Legion.Core.Threading;

using Newtonsoft.Json.Linq;

namespace Heliu.Core.Kafka
{
    public class KafkaMessageListener : IMessageListener
    {
        private readonly KafkaConfig config;
        private readonly string listenerGroupId;
        private readonly IMessageTypeRegistry messageTypeRegistry;
        private HandleMessageAsync handleMessageObject;
        private readonly long? consumeFromOffset;

        public KafkaMessageListener(KafkaConfig config, string listenerGroupId, long? consumeFromOffset, IMessageTypeRegistry messageTypeRegistry)
        {
            this.consumeFromOffset = consumeFromOffset;
            this.config = config;
            this.listenerGroupId = listenerGroupId;
            this.messageTypeRegistry = messageTypeRegistry;
        }

        public async Task StartAsync(string topic, HandleMessageAsync handleMessageObject, CancellationToken? cancellationToken = null)
        {
            this.handleMessageObject = handleMessageObject;

            var conf = new Dictionary<string, object>
                                    {
                                        { "group.id", this.listenerGroupId },
                                        { "bootstrap.servers", this.config.BootstrapServers },
                                        { "auto.commit.interval.ms", 5000 },
                                        { "auto.offset.reset", "earliest" }
                                    };

            Console.WriteLine("Listening to Kafka messages");
            using (var consumer = new Consumer<Null, string>(conf, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.OnError += (_, error)
                  => Console.WriteLine($"Error: {error}");

                consumer.OnConsumeError += (_, msg)
                  => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");


                if (this.consumeFromOffset.HasValue)
                {
                    var topicPartitionOffset = new TopicPartitionOffset(new TopicPartition(topic, 0), new Offset(this.consumeFromOffset.Value));
                    consumer.Assign(new[] { topicPartitionOffset });

                }
                else
                {
                    consumer.Subscribe(topic);
                }

                while (cancellationToken.KeepRunning())
                {
                    Message<Null, string> msg;
                    if (!consumer.Consume(out msg, TimeSpan.FromMilliseconds(100)))
                    {
                        continue;
                    }

                    Console.WriteLine($"Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {msg.Value}");

                    await HandleMessage(msg);

                    Console.WriteLine($"Committing offset");
                    var committedOffsets = consumer.CommitAsync(msg).Result;
                    Console.WriteLine($"Committed offset: {committedOffsets}");
                }
            }
        }

        private async Task HandleMessage(Message<Null, string> message)
        {
            var parsedMessage = JObject.Parse(message.Value);
            var messageTypeName = parsedMessage.GetValue("MessageType").ToString();
            var messageType = this.messageTypeRegistry.GetMessageType(messageTypeName);
            var messageObject = parsedMessage.ToObject(messageType);

            await this.handleMessageObject(null, messageObject);
        }
    }
}