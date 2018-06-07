using System.Collections.Generic;
using System.Linq;

namespace Legion.Core.Messages.InMemory
{
    public class InMemoryMessageStore
    {
        private Dictionary<string, InMemoryQueue> queues = new Dictionary<string, InMemoryQueue>();

        public int AddMessage(string topic, object header, object message)
        {
            var queue = this.GetQueue(topic);

            return queue.AddMessage(header, message);
        }

        public object GetMessage(string topic, int index)
        {
            var queue = this.GetQueue(topic);

            return queue.GetMessage(index);
        }

        public object GetMessageHeader(string topic, int index)
        {
            var queue = this.GetQueue(topic);

            return queue.GetMessageHeader(index);
        }

        public int GetLastIndex(string topic)
        {
            var queue = this.GetQueue(topic);

            return queue.LastIndex();
        }

        private InMemoryQueue GetQueue(string topic)
        {
            InMemoryQueue queue = null;
            if (!this.queues.TryGetValue(topic, out queue))
            {
                queue = new InMemoryQueue();
                this.queues.Add(topic, queue);
            }

            return queue;
        }
    }
}