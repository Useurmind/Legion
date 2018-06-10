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

        public object Deserialize(byte[] storedMessageFormat)
        {
            var messageString = Encoding.UTF8.GetString(storedMessageFormat);

            var jobject = JObject.Parse(messageString);
            var typeName = jobject["@type"].Value<string>();

            var messageType = this.messageTypeRegistry.GetMessageType(typeName);

            return jobject.ToObject(messageType);
        }

        public byte[] Serialize(object runtimeMessageFormat)
        {
            var jobject = JObject.FromObject(runtimeMessageFormat);
            var typeName = this.messageTypeRegistry.GetMessageTypeName(runtimeMessageFormat.GetType());

            jobject.Add("@type", typeName);

            var messageString = jobject.ToString(Formatting.None);
            return Encoding.UTF8.GetBytes(messageString);
        }
    }
}
