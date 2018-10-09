using System.Collections.Generic;
using System.Linq;

namespace Legion.Core.Messages.Serialization
{
    public class SerializedMessage : ISerializedMessage
    {
        public byte[] Key { get; set; }
        public byte[] Value { get; set; }
        public IDictionary<string, byte[]> Headers { get; } = new Dictionary<string, byte[]>();
    }
}