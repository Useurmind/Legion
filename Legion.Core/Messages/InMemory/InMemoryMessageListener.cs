using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Legion.Core.Threading;

namespace Legion.Core.Messages.InMemory
{
    public class InMemoryMessageListener : IMessageListener
    {
        private readonly InMemoryMessageStore inMemoryMessageStore;

        public InMemoryMessageListener(InMemoryMessageStore inMemoryMessageStore)
        {
            this.inMemoryMessageStore = inMemoryMessageStore;
        }

        /// <inheritdoc />
        public async Task StartAsync(string topic, HandleMessageAsync handleMessageObject, CancellationToken? cancellationToken = null)
        {
            var currentIndex = -1;

            while (cancellationToken.KeepRunning())
            {
                while (cancellationToken.KeepRunning() && currentIndex >= this.inMemoryMessageStore.GetLastIndex(topic))
                {
                    Thread.Sleep(100);
                }

                if (!cancellationToken.KeepRunning())
                {
                    break;
                }

                currentIndex++;

                var message = this.inMemoryMessageStore.GetMessage(topic, currentIndex);
                var messageHeader = this.inMemoryMessageStore.GetMessageHeader(topic, currentIndex);

                await handleMessageObject(messageHeader, message);
            }

            int a = 1;
        }
    }
}