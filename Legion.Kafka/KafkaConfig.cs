using System.Collections.Generic;
using System.Linq;

namespace Legion.Kafka
{
    public class KafkaConfig : Dictionary<string, object>
    {
        public string BootstrapServers
        {
            get
            {
                return (string)this["bootstrap.servers"];
            }
        }
    }
}