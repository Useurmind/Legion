using System;
using System.Collections.Generic;
using System.Text;

using Confluent.Kafka;

using Heliu.Core.Kafka;

using Legion.Core.Configuration;
using Legion.Core.Messages.Serialization;
using Legion.Core.Messages.Types;

namespace Legion.Kafka
{
    public static class KafkaDependencyInjectionExtensions
    {
        /// <summary>
        /// Add the kafka listener to the container to enable consuming topics with your handlers.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="kafkaConfig"></param>
        /// <param name="consumeFromOffset">A dictionary with indexes for topics from which to start (does not need to be complete, topics without explicit indexes are restarted from the listener group index).</param>
        /// <returns></returns>
        public static IDependencyRegistrationContext AddKafkaListener(this IDependencyRegistrationContext context, ConsumerConfig kafkaConfig, string listenerGroupId, Dictionary<string, long> consumeFromOffset)
        {
            context.RegisterSingleton(c => new Consumer<byte[], byte[]>(kafkaConfig), typeof(Consumer<byte[], byte[]>));
            context.RegisterTransient(c => new KafkaMessageListener(c.Resolve<Consumer<byte[], byte[]>>(), listenerGroupId, consumeFromOffset, c.Resolve<IMessageSerializer>()));

            return context;
        }
        /// <summary>
        /// Add the kafka listener to the container to enable consuming topics with your handlers.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="kafkaConfig"></param>
        /// <returns></returns>
        public static IDependencyRegistrationContext AddKafkaSender(this IDependencyRegistrationContext context, ProducerConfig kafkaConfig)
        {
            context.RegisterSingleton(c => new Producer<byte[], byte[]>(kafkaConfig), typeof(Producer<byte[], byte[]>));
            context.RegisterTransient(c => new KafkaMessageSender(c.Resolve<Producer<byte[], byte[]>>(), c.Resolve<IMessageSerializer>()));

            return context;
        }
    }
}
