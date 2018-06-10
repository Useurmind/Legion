using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Legion.Core.Threading;

namespace Legion.Core.Messages.InMemory
{
    public class InMemoryMessageListener : IMessageListener
    {
        private Dictionary<string, int> currentIndexes;
        private readonly InMemoryMessageStore inMemoryMessageStore;

        private HandleMessageAsync handleMessageObject;

        public InMemoryMessageListener(InMemoryMessageStore inMemoryMessageStore)
        {
            this.inMemoryMessageStore = inMemoryMessageStore;
        }

        /// <inheritdoc />
        public async Task StartAsync(IEnumerable<string> topics, HandleMessageAsync handleMessageObject, CancellationToken? cancellationToken = null)
        {
            this.currentIndexes = topics.ToDictionary(x => x, x => -1);
            this.handleMessageObject = handleMessageObject;

            while (cancellationToken.KeepRunning())
            {
                var updatedTopics = this.GetTopicsWithNewMessages();
                while (cancellationToken.KeepRunning() && !updatedTopics.Any())
                {
                    Thread.Sleep(100);
                    updatedTopics = this.GetTopicsWithNewMessages();
                }

                if (!cancellationToken.KeepRunning())
                {
                    break;
                }

                foreach (var topic in updatedTopics)
                {
                    await this.HandleMessageInTopic(topic);
                }
            }
        }

        private IEnumerable<string> GetTopicsWithNewMessages()
        {
            return this.currentIndexes.Keys.Where(x => this.HasTopicNewMessage(x)).ToArray();
        }

        private bool HasTopicNewMessage(string topic)
        {
            return this.currentIndexes[topic] < this.inMemoryMessageStore.GetLastIndex(topic);
        }

        private async Task HandleMessageInTopic(string topic)
        {
            var currentIndex = this.currentIndexes[topic];

            currentIndex++;

            var message = this.inMemoryMessageStore.GetMessage(topic, currentIndex);
            var messageHeader = this.inMemoryMessageStore.GetMessageHeader(topic, currentIndex);
            var messageKey = this.inMemoryMessageStore.GetMessageKey(topic, currentIndex);

            await this.handleMessageObject(messageKey, messageHeader, message);
        }
    }
}