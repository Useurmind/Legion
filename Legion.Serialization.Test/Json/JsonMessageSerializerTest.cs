using System.Linq;

using FluentAssertions;

using Legion.Core.Messages.Types;
using Legion.Serialization.Json;

using NSubstitute;

using Xunit;

namespace Legion.Serialization.Test.Json
{
    public class JsonMessageSerializerTest
    {
        private class TestMessage
        {
            public int Id { get; set; }

            public string SomeString { get; set; }
        }

        [Fact]
        public void SerializerCanSerializeAndDeserializeMessage()
        {
            var messageTypeRegistry = Substitute.For<IMessageTypeRegistry>();
            var messageTypeName = "TestMessageType";
            messageTypeRegistry.GetMessageType(messageTypeName).Returns(typeof(TestMessage));
            messageTypeRegistry.GetMessageTypeName(typeof(TestMessage)).Returns(messageTypeName);

            var serializer = new JsonMessageSerializer(messageTypeRegistry);

            var testMessage= new TestMessage()
                                 {
                                     Id = 3,
                                     SomeString = "aklsjdöas"
                                 };

            var serialized = serializer.Serialize(testMessage);
            var deserialized = (TestMessage)serializer.Deserialize(serialized);

            deserialized.Id.Should().Be(testMessage.Id);
            deserialized.SomeString.Should().Be(testMessage.SomeString);
        }
    }
}
