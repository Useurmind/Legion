using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Legion.Core.Messages.Consume
{
    /// <summary>
    /// Interface for a class that runs a dispatch loop to dispatch messages to different handlers.
    /// </summary>
    public interface IMessageDispatcher{
        /// <summary>
        /// Run the dispatch loop asynchronously.
        /// </summary>
        /// <param name="topic">The topic to dispatch messages for.</param>
        /// <param name="cancellationToken">Cancellation token to cancel dispatching.</param>
        /// <returns></returns>
        Task RunDispatchLoopAsync(string topic, CancellationToken? cancellationToken);
    }
}