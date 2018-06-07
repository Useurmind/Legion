using System;
using System.Linq;
using System.Threading.Tasks;

namespace Legion.Core.Messages.Handler
{
    /// <summary>
    /// Base class for message handlers.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class MessageHandlerBase<TMessage> : IMessageHandler<TMessage>
    {
        /// <inheritdoc />
        public Type MessageType
        {
            get
            {
                return typeof(TMessage);
            }
        }

        /// <inheritdoc />
        public async Task HandleMessageAsync(object evt)
        {
            await this.HandleMessageAsync((TMessage)evt);
        }

        /// <inheritdoc />
        public abstract Task HandleMessageAsync(TMessage evt);
    }
}