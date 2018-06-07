using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Legion.Core.Messages.Types;

namespace Legion.Core.Messages
{
    /// <summary>
    /// Type for delegates that handle messages.
    /// </summary>
    /// <param name="header">The header as C# object.</param>
    /// <param name="message">The message as C# object.</param>
    /// <returns></returns>
    public delegate Task HandleMessageAsync(object header, object message);

    /// <summary>
    /// This interface serves as the listener interface for implementations of different message stores.
    /// 
    /// </summary>
    public interface IMessageListener
    {
        /// <summary>
        /// Start the listener asynchronously.
        /// </summary>
        /// <param name="topic">The name of the topic to listen on.</param>
        /// <param name="handleMessageObject">A function that handles any new message objects. The message objects should
        /// already have been converted to the corresponding C# types (e.g. by using <see cref="IMessageTypeRegistry" />)</param>
        /// <param name="cancellationToken">Cancellation token to cancel the listening.</param>
        /// <returns>Task to control async behaviour</returns>
        Task StartAsync(string topic, HandleMessageAsync handleMessageObject, CancellationToken? cancellationToken = null);
    }
}