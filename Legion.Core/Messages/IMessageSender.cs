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
        /// <param name="header">A c# object representing a header.</param>
        /// <param name="message">A c# object representing a message.</param>
        /// <returns></returns>
        Task SendMessageAsync(string topic, object header, object message);
    }
}