using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Legion.Core.Messages.InMemory;

using Xunit;

namespace Legion.Core.Test.Messages.InMemory
{
    public class InMemoryMessageListenerTest
    {
        [Fact]
        public async Task MessagesInStoreAreReceived()
        {
            var topic = "askldjfdg";
            var message1 = new object();
            var message2 = new object();
            var message3 = new object();
            var header1 = new object();
            var header2 = new object();
            var header3 = new object();
            var key1 = new object();
            var key2 = new object();
            var key3 = new object();

            var headerList = new List<object>();
            var messageList = new List<object>();
            var keyList = new List<object>();

            var messageStore = new InMemoryMessageStore();
            var messageListener = new InMemoryMessageListener(messageStore);

            messageStore.AddMessage(topic, key1, header1, message1);
            messageStore.AddMessage(topic, key2, header2, message2);
            messageStore.AddMessage(topic, key3, header3, message3);

            await messageListener.StartAsync(
                new[] { topic },
                async (k, h, m) =>
                    {
                        keyList.Add(k);
                        headerList.Add(h);
                        messageList.Add(m);
                    },
                new CancellationTokenSource(100).Token);

            headerList.Should().ContainInOrder(header1, header2, header3);
            messageList.Should().ContainInOrder(message1, message2, message3);
            keyList.Should().ContainInOrder(key1, key2, key3);
        }
    }
}