using System;
using System.Collections.Generic;
using System.Text;

using FluentAssertions;

using Legion.Core.Messages.InMemory;

using Xunit;

namespace Legion.Core.Test.InMemory
{
    public class InMemoryQueueTest
    {
        [Fact]
        public void AddingMessagesIncreasesIndex()
        {
            var queue = new InMemoryQueue();

            var index1 = queue.AddMessage(null, new object());
            var index2 = queue.AddMessage(null, new object());
            var index3 = queue.AddMessage(null, new object());

            index1.Should().Be(0);
            index2.Should().Be(1);
            index3.Should().Be(2);
        }

        [Fact]
        public void AddingNullHeaderAlsoReturnsNullHeader()
        {
            var queue = new InMemoryQueue();

            var index1 = queue.AddMessage(null, new object());

            queue.GetMessageHeader(index1).Should().BeNull();
        }

        [Fact]
        public void AddedMessagesCanBeRetrieved()
        {
            var queue = new InMemoryQueue();
            var message1 = new object();
            var message2 = new object();
            var message3 = new object();

            var index1 = queue.AddMessage(null, message1);
            var index2 = queue.AddMessage(new object(), message2);
            var index3 = queue.AddMessage(new object(), message3);

            queue.GetMessage(index1).Should().Be(message1);
            queue.GetMessage(index2).Should().Be(message2);
            queue.GetMessage(index3).Should().Be(message3);
        }

        [Fact]
        public void AddedHeadersCanBeRetrieved()
        {
            var queue = new InMemoryQueue();
            var header1 = new object();
            var header2 = new object();
            var header3 = new object();

            var index1 = queue.AddMessage(header1, new object());
            var index1a = queue.AddMessage(null, new object());
            var index2 = queue.AddMessage(header2, new object());
            var index3 = queue.AddMessage(header3, new object());

            queue.GetMessageHeader(index1).Should().Be(header1);
            queue.GetMessageHeader(index2).Should().Be(header2);
            queue.GetMessageHeader(index3).Should().Be(header3);

        }
    }
}
