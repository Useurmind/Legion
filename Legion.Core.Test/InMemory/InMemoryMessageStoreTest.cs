using System;
using System.Collections.Generic;
using System.Text;

using FluentAssertions;

using Legion.Core.Messages.InMemory;

using Xunit;

namespace Legion.Core.Test.InMemory
{
    public class InMemoryMessageStoreTest
    {
        [Fact]
        public void StoredMessagesCanBeRetrieved()
        {
            var topic = "askldjfdg";
            var topic2 = "askldjfasdasdg";
            var message1 = new object();
            var message2 = new object();
            var message3 = new object();
            var header1 = new object();
            var header2 = new object();
            var header3 = new object();
            var messageStore = new InMemoryMessageStore();

            var index1 = messageStore.AddMessage(topic, header1, message1);
            var index2 = messageStore.AddMessage(topic2, header2, message2);
            var index3 = messageStore.AddMessage(topic, header3, message3);

            index1.Should().Be(0);
            index2.Should().Be(0);
            index3.Should().Be(1);

            messageStore.GetMessage(topic, 0).Should().Be(message1);
            messageStore.GetMessage(topic2, 0).Should().Be(message2);
            messageStore.GetMessage(topic, 1).Should().Be(message3);

            messageStore.GetMessageHeader(topic, 0).Should().Be(header1);
            messageStore.GetMessageHeader(topic2, 0).Should().Be(header2);
            messageStore.GetMessageHeader(topic, 1).Should().Be(header3);
        }
    }
}
