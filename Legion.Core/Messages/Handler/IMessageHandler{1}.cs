using System.Linq;
using System.Threading.Tasks;

namespace Legion.Core.Messages.Handler
{
    /// <summary>
    /// Interface for a class handling messages of a specific type.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageHandler<TMessage> : IMessageHandler
    {
        /// <summary>
        /// Handle the message asynchronously.
        /// </summary>
        /// <param name="evt">Message to handle.</param>
        /// <returns></returns>
        Task HandleMessageAsync(TMessage evt);
    }
}