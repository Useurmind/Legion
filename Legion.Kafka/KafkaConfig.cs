using System.Collections.Generic;

namespace Heliu.Core.Kafka
{
    public class KafkaConfig : Dictionary<string, object>
    {
        public string BootstrapServers { get; set; }
    }
}