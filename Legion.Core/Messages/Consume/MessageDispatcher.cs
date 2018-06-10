using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Legion.Core.Messages.Handler;

namespace Legion.Core.Messages.Consume
{
    /// <summary>
    /// An message dispatcher that runs a loop to handle messages.
    /// It receives message from an message listener which is a messaging dependent component (e.g. kafka).
    /// </summary>
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IMessageListener messageListener;
        private readonly IMessageHandlerRegistry messageHandlerRegistry;

        public MessageDispatcher(IMessageListener messageListener, IMessageHandlerRegistry messageHandlerRegistry)
        {
            this.messageListener = messageListener;
            this.messageHandlerRegistry = messageHandlerRegistry;
        }

        /// <inheritdoc />
        public async Task RunDispatchLoopAsync(IEnumerable<string> topics, CancellationToken? cancellationToken = null)
        {
            await this.messageListener.StartAsync(topics, this.HandleMessageObject, cancellationToken);
        }

        private async Task HandleMessageObject(object key, object header, object messageObject)
        {
            Console.WriteLine($"Handling {messageObject.GetType().Name}");
            var messageHandler = this.messageHandlerRegistry.GetMessageHandler(messageObject.GetType());

            foreach(var handler in messageHandler) {
                Console.WriteLine($"Applying handler {handler.GetType().Name}");
                await handler.HandleMessageAsync(messageObject);
            }

            Console.WriteLine($"Handled {messageObject.GetType().Name}");
        }
    }
}