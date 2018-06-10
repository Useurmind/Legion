using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Legion.Core.Messages;
using Legion.Core.Messages.Consume;
using Legion.Core.Messages.Handler;
using Legion.Core.Messages.InMemory;
using Legion.Core.Messages.Types;

namespace Legion.Core.Configuration
{
    /// <summary>
    /// Class that contains extension methods for configuring dependency injection.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Use a simple in memory storage for transmitting messages.
        /// Good for testing.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDependencyRegistrationContext UseInMemoryStorage(this IDependencyRegistrationContext context)
        {
            context.RegisterSingleton(c => new InMemoryMessageStore(), typeof(InMemoryMessageStore));
            context.RegisterTransient(c => new InMemoryMessageSender(c.Resolve<InMemoryMessageStore>()), typeof(IMessageSender));
            context.RegisterTransient(c => new InMemoryMessageListener(c.Resolve<InMemoryMessageStore>()), typeof(IMessageListener));

            return context;
        }

        /// <summary>
        /// Tell legion to scan for message types that are marked with the <see cref="MessageAttribute"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembliesToScan">The assemblies that should be scanned for messages (if null only entry assembly is scanned).</param>
        /// <returns></returns>
        public static IDependencyRegistrationContext ScanForMessages(
            this IDependencyRegistrationContext context,
            IEnumerable<Assembly> assembliesToScan = null)
        {
            if (assembliesToScan == null)
            {
                assembliesToScan = new[] { Assembly.GetEntryAssembly() }.Distinct();
            }

            context.RegisterSingleton(c => new MessageTypeRegistry(assembliesToScan), typeof(IMessageTypeRegistry));

            return context;
        }

        /// <summary>
        /// Tell legion to scan for message types that are marked with the <see cref="MessageAttribute"/>.
        /// </summary>
        /// <typeparam name="TSibling">A type whose assembly should be scanned.</typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDependencyRegistrationContext ScanForMessages<TSibling>(this IDependencyRegistrationContext context)
        {
            context.ScanForMessages(new[] { typeof(TSibling).Assembly });

            return context;
        }

        /// <summary>
        /// Tell legion to scan for message handlers that are marked with the <see cref="MessageHandlerAttribute"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembliesToScan">The assemblies that should be scanned for message handlers (if null only entry assembly is scanned).</param>
        /// <returns></returns>
        public static IDependencyRegistrationContext ScanForMessageHandlers(
            this IDependencyRegistrationContext context,
            IEnumerable<Assembly> assembliesToScan = null)
        {
            if (assembliesToScan == null)
            {
                assembliesToScan = new[] { Assembly.GetEntryAssembly() }.Distinct();
            }

            context.RegisterTransient(
                       c => new MessageDispatcher(c.Resolve<IMessageListener>(), c.Resolve<IMessageHandlerRegistry>()),
                       typeof(IMessageDispatcher));

            var messageHandlerRegistry = new MessageHandlerRegistry();

            context.RegisterSingleton(c =>
                {
                    messageHandlerRegistry.ScanForMessageHandlerTypes(assembliesToScan, t => context.RegisterTransient(t, typeof(IMessageHandler), t));
                    var resolutionContext = c.ResolveForLater();
                    messageHandlerRegistry.ConnectContainerResolution(t => (IMessageHandler)resolutionContext.Resolve(t));
                    return messageHandlerRegistry;
                });

            return context;
        }

        /// <summary>
        /// Tell legion to scan for message handlers that are marked with the <see cref="MessageHandlerAttribute"/>.
        /// </summary>
        /// <typeparam name="TSibling">A type whose assembly should be scanned.</typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDependencyRegistrationContext ScanForMessageHandlers<TSibling>(this IDependencyRegistrationContext context)
        {
            context.ScanForMessageHandlers(new[] { typeof(TSibling).Assembly });

            return context;
        }
    }
}
