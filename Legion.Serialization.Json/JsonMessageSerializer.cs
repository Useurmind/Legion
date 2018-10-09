using System;
using System.Text;

using Legion.Core.Messages.Serialization;
using Legion.Core.Messages.Types;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Legion.Serialization.Json
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        private readonly IMessageTypeRegistry messageTypeRegistry;

        public JsonMessageSerializer(IMessageTypeRegistry messageTypeRegistry)
        {
            this.messageTypeRegistry = messageTypeRegistry;
        }

        public object Deserialize(ISerializedMessage storedMessageFormat)
        {
            var messageString = Encoding.UTF8.GetString(storedMessageFormat.Value);
            var typeName = Encoding.UTF8.GetString(storedMessageFormat.Headers["type"]);

            var messageType = this.messageTypeRegistry.GetMessageType(typeName);

            return JsonConvert.DeserializeObject(messageString, messageType);
        }

        public ISerializedMessage Serialize(object runtimeMessageFormat)
        {
            var messageString = JsonConvert.SerializeObject(runtimeMessageFormat);
            var typeName = this.messageTypeRegistry.GetMessageTypeName(runtimeMessageFormat.GetType());
            
            var serializedMessage = new SerializedMessage();
            serializedMessage.Value = Encoding.UTF8.GetBytes(messageString);
            serializedMessage.Headers.Add("type", Encoding.UTF8.GetBytes(typeName));

            return serializedMessage;
        }

        public byte[] SerializeKey(object keyValue)
        {
            return this.ConvertToJsonByteArray(keyValue);
        }

        public byte[] SerializeHeaderValue(object headerValue)
        {
            return this.ConvertToJsonByteArray(headerValue);
        }

        public object DeserializeKey(byte[] keyBytes)
        {
            throw new NotImplementedException();
        }

        public object DeserializeHeaderValue(byte[] headerValueBytes)
        {
            throw new NotImplementedException();
        }

        private byte[] ConvertToJsonByteArray(object value)
        {
            var jsonString = JsonConvert.SerializeObject(value);
            return Encoding.UTF8.GetBytes(jsonString);
        }
    }
}
