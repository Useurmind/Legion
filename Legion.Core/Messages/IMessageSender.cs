using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Legion.Core.Messages
{
    /// <summary>
    /// Simple sender to send messages to a topic on an message store.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Send a message to a queue.
        /// Both header and message should be serializable using the configured serializer.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message">A c# object representing a message.</param>
        /// <param name="key">A C# object representing the key of the message (we usually asume the key is computed from the message object, so it can be null).</param>
        /// <param name="headers">A dictionary of already serialized header values (headers can be handed down here and will be combined with more headers from the message serializer).</param>
        /// <returns></returns>
        Task SendMessageAsync(string topic, object message, object key=null, IDictionary<string, object> headers=null);
    }
}