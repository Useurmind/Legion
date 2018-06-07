using System;
using System.Collections.Generic;
using System.Linq;

namespace Legion.Core.Messages.Handler
{
    /// <summary>
    /// This registry provides handlers for the different types of messages.
    /// </summary>
    public interface IMessageHandlerRegistry
    {
        IEnumerable<IMessageHandler> GetMessageHandler(Type messageType);
    }
}