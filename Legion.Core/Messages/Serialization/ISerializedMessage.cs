using System.Collections.Generic;
using System.Linq;

namespace Legion.Core.Messages.Serialization
{
    public interface ISerializedMessage
    {
        byte[] Key { get; }

        byte[] Value { get; }

        IDictionary<string, byte[]> Headers { get; }
    }
}