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

            var headerList = new List<object>();
            var messageList = new List<object>();

            var messageStore = new InMemoryMessageStore();
            var messageListener = new InMemoryMessageListener(messageStore);

            messageStore.AddMessage(topic, header1, message1);
            messageStore.AddMessage(topic, header2, message2);
            messageStore.AddMessage(topic, header3, message3);

            await messageListener.StartAsync(topic,
                async (h, m) =>
                    {
                        headerList.Add(h);
                        messageList.Add(m);
                    }, new CancellationTokenSource(100).Token);

            headerList.Should().ContainInOrder(header1, header2, header3);
            messageList.Should().ContainInOrder(message1, message2, message3);
        }
    }
}
