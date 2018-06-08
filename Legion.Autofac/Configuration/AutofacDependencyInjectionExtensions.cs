using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;

using Legion.Autofac.Modules;
using Legion.Core.Messages.Handler;
using Legion.Core.Messages.Types;

namespace Legion.Autofac.Configuration
{
    public static class AutofacDependencyInjectionExtensions
    {
        /// <summary>
        /// Start to configure legion usage.
        /// </summary>
        /// <param name="containerBuilder">The autofac container builder.</param>
        /// <returns></returns>
        public static IDependencyInjectionContext AddLegion(this ContainerBuilder containerBuilder)
        {
            return new AutofacDependencyInjectionContext(containerBuilder);
        }

        /// <summary>
        /// Tell legion to scan for message types that are marked with the <see cref="MessageAttribute"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembliesToScan">The assemblies that should be scanned for messages (if null only entry assembly is scanned).</param>
        /// <returns></returns>
        public static IDependencyInjectionContext ScanForMessages(
            this IDependencyInjectionContext context,
            IEnumerable<Assembly> assembliesToScan = null)
        {
            context.ContainerBuilder.RegisterModule(new ScannedMessagesModule(assembliesToScan));

            return context;
        }

        /// <summary>
        /// Tell legion to scan for message types that are marked with the <see cref="MessageAttribute"/>.
        /// </summary>
        /// <typeparam name="TSibling">A type whose assembly should be scanned.</typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDependencyInjectionContext ScanForMessages<TSibling>(this IDependencyInjectionContext context)
        {
            context.ContainerBuilder.RegisterModule(new ScannedMessagesModule(new [] { typeof(TSibling).Assembly }));

            return context;
        }

        /// <summary>
        /// Tell legion to scan for message handlers that are marked with the <see cref="MessageHandlerAttribute"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembliesToScan">The assemblies that should be scanned for message handlers (if null only entry assembly is scanned).</param>
        /// <returns></returns>
        public static IDependencyInjectionContext ScanForMessageHandlers(
            this IDependencyInjectionContext context,
            IEnumerable<Assembly> assembliesToScan = null)
        {
            context.ContainerBuilder.RegisterModule(new ScannedMessageHandlersModule(assembliesToScan));

            return context;
        }

        /// <summary>
        /// Tell legion to scan for message handlers that are marked with the <see cref="MessageHandlerAttribute"/>.
        /// </summary>
        /// <typeparam name="TSibling">A type whose assembly should be scanned.</typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDependencyInjectionContext ScanForMessageHandlers<TSibling>(this IDependencyInjectionContext context)
        {
            context.ContainerBuilder.RegisterModule(new ScannedMessageHandlersModule(new[] { typeof(TSibling).Assembly }));

            return context;
        }
    }
}