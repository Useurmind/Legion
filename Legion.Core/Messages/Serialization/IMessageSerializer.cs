using System;
using System.Text;

namespace Legion.Core.Messages.Serialization
{
    public interface IMessageSerializer
    {
        object Deserialize(ISerializedMessage storedMessageFormat);

        ISerializedMessage Serialize(object runtimeMessageFormat);

        byte[] SerializeKey(object keyValue);

        byte[] SerializeHeaderValue(object headerValue);

        object DeserializeKey(byte[] keyBytes);

        object DeserializeHeaderValue(byte[] headerValueBytes);
    }
}
