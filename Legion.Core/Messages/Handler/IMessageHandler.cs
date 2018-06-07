using System;
using System.Linq;
using System.Threading.Tasks;

namespace Legion.Core.Messages.Handler
{
    /// <summary>
    /// Generic interface for a class that handles messages.
    /// </summary>
    public interface IMessageHandler
    {
        /// <summary>
        /// The type of the message that is handled.
        /// </summary>
        Type MessageType { get; }

        /// <summary>
        /// Handle the message asynchronously.
        /// </summary>
        /// <param name="evt">Message to handle.</param>
        /// <returns></returns>
        Task HandleMessageAsync(object evt);
    }
}