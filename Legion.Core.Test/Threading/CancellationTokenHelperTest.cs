using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using FluentAssertions;

using Legion.Core.Threading;

using Xunit;

namespace Legion.Core.Test.Threading
{
    public class CancellationTokenHelperTest
    {
        [Fact]
        public void KeepRunningIsTrueWhenCancellationTokenNull()
        {
            CancellationToken? token = null;

            token.KeepRunning().Should().BeTrue();
        }

        [Fact]
        public void KeepRunningIsTrueWhenCancellationTokenFresh()
        {
            CancellationToken? token = new CancellationTokenSource().Token;

            token.KeepRunning().Should().BeTrue();
        }

        [Fact]
        public void KeepRunningIsTrueWhenCancellationTokenCancelled()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken? token = cancellationTokenSource.Token;

            cancellationTokenSource.Cancel();
            
            token.KeepRunning().Should().BeFalse();
        }
    }
}
