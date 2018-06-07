//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Confluent.Kafka;
//using Confluent.Kafka.Serialization;

//using Legion.Core.Messages;

//namespace Heliu.Core.Kafka {
//    public class KafkaSender : IMessageSender {
//        private KafkaConfig config;

//        public KafkaSender(KafkaConfig config)
//        {
//            this.config = config;
//        }

//        public async Task SendMessageAsync(string topic, object header, object message) {
//            var config = new Dictionary<string, object> 
//            { 
//                { "bootstrap.servers", this.config.BootstrapServers } 
//            };

//            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
//            {
//                var dr = producer.ProduceAsync((topic, null, text).Result;
//                Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
//            }
//        }
//    }
//}