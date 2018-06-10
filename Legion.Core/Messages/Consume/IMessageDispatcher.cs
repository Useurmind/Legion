using System.Collections.Generic;
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
        /// <param name="topics">The topics to dispatch messages for.</param>
        /// <param name="cancellationToken">Cancellation token to cancel dispatching.</param>
        /// <returns></returns>
        Task RunDispatchLoopAsync(IEnumerable<string> topics, CancellationToken? cancellationToken);
    }
}