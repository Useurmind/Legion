using System;
using System.Collections.Generic;
using System.Text;

namespace Legion.Core.Messages.Serialization
{
    public interface IMessageSerializer
    {
        object Deserialize(byte[] storedMessageFormat);

        byte[] Serialize(object runtimeMessageFormat);
    }
}
