using System.Collections.Generic;
using System.Linq;

namespace Legion.Kafka
{
    public static class KafkaConfigExtensions
    {
        public static KafkaConfig BootstrapServers(this KafkaConfig kafkaConfig, IEnumerable<string> bootstrapServers)
        {
            kafkaConfig.BootstrapServers(string.Join(",", bootstrapServers));

            return kafkaConfig;
        }

        public static KafkaConfig BootstrapServers(this KafkaConfig kafkaConfig, string bootstrapServers)
        {
            kafkaConfig["bootstrap.servers"] = bootstrapServers;

            return kafkaConfig;
        }

        public static KafkaConfig ListenerGroupId(this KafkaConfig kafkaConfig, string groupId)
        {
            kafkaConfig["group.id"] = groupId;

            return kafkaConfig;
        }

        public static KafkaConfig EnableAutoCommit(this KafkaConfig kafkaConfig, bool enableAutoCommit)
        {
            kafkaConfig["enable.auto.commit"] = enableAutoCommit;

            return kafkaConfig;
        }

        public static KafkaConfig AutoCommitIntervalMs(this KafkaConfig kafkaConfig, bool autoCommitIntervalMs)
        {
            kafkaConfig["auto.commit.interval.ms"] = autoCommitIntervalMs;

            return kafkaConfig;
        }

        public static KafkaConfig AutoOffsetReset(this KafkaConfig kafkaConfig, AutoOffsetRest autoOffsetReset)
        {
            kafkaConfig.AutoOffsetReset(autoOffsetReset.ToString().ToLowerInvariant());

            return kafkaConfig;
        }

        public static KafkaConfig AutoOffsetReset(this KafkaConfig kafkaConfig, string autoOffsetReset)
        {
            kafkaConfig["auto.offset.reset"] = autoOffsetReset;

            return kafkaConfig;
        }
    }

    public enum AutoOffsetRest
    {
        Earliest,
        Latest
    }
}