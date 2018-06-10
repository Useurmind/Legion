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
        public static IDependencyRegistrationContext AddKafkaListener(this IDependencyRegistrationContext context, KafkaConfig kafkaConfig, string listenerGroupId, Dictionary<string, long> consumeFromOffset)
        {
            context.RegisterSingleton(c => new Consumer(kafkaConfig), typeof(Consumer));
            context.RegisterTransient(c => new KafkaMessageListener(c.Resolve<Consumer>(), listenerGroupId, consumeFromOffset, c.Resolve<IMessageSerializer>()));

            return context;
        }
        /// <summary>
        /// Add the kafka listener to the container to enable consuming topics with your handlers.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="kafkaConfig"></param>
        /// <returns></returns>
        public static IDependencyRegistrationContext AddKafkaSender(this IDependencyRegistrationContext context, KafkaConfig kafkaConfig)
        {
            context.RegisterSingleton(c => new Producer(kafkaConfig), typeof(Producer));
            context.RegisterTransient(c => new KafkaMessageSender(c.Resolve<Producer>(), c.Resolve<IMessageSerializer>()));

            return context;
        }
    }
}
