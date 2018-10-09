using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Legion.Core.Messages.InMemory
{
    public class InMemoryMessageSender : IMessageSender
    {
        private readonly InMemoryMessageStore inMemoryMessageStore;

        public InMemoryMessageSender(InMemoryMessageStore inMemoryMessageStore)
        {
            this.inMemoryMessageStore = inMemoryMessageStore;
        }

        public async Task SendMessageAsync(string topic, object message, object key, IDictionary<string, object> header)
        {
            this.inMemoryMessageStore.AddMessage(topic, key, header, message);
        }
    }
}
