using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Legion.Core.Threading
{
    public static class CancellationTokenHelper
    {
        /// <summary>
        /// Indicates whether to keep running.
        /// If set then only when no cancellation was requested.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static bool KeepRunning(this CancellationToken? cancellationToken)
        {
            if (!cancellationToken.HasValue)
            {
                return true;
            }

            return !cancellationToken.Value.IsCancellationRequested;
        }
    }
}