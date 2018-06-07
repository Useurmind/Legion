using Autofac;

using Legion.Core.Messages;
using Legion.Core.Messages.Types;

namespace Heliu.Core.Kafka
{
    public class KafkaModule : Module
    {
        private readonly string listenerGroupId;
        private readonly KafkaConfig config;
        private readonly long? consumeFromOffset;

        public KafkaModule(string bootstrapServers, string listenerGroupId = "heliuDefaultListener", long? consumeFromOffset = null)
        {
            this.consumeFromOffset = consumeFromOffset;
            this.listenerGroupId = listenerGroupId;
            this.config = new KafkaConfig
            {
                BootstrapServers = bootstrapServers
            };
        }

        override protected void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(this.config);
            //builder.Register(c => new KafkaTextSender(c.Resolve<KafkaConfig>())).As<IMessageSender>();
            builder.Register(c => new KafkaMessageListener(
                    c.Resolve<KafkaConfig>(),
                    this.listenerGroupId,
                    this.consumeFromOffset,
                    c.Resolve<IMessageTypeRegistry>()))
                .As<IMessageListener>();
        }
    }
}