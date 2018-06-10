using System;
using System.Linq;

namespace Legion.Core.Messages.Types
{
    /// <summary>
    /// Interface that delivers types into which messages can be converted.
    /// </summary>
    public interface IMessageTypeRegistry
    {
        Type GetMessageType(string messageTypeName);

        string GetMessageTypeName(Type messageType);
    }
}