using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;

using Legion.Core.Messages;
using Legion.Core.Messages.Consume;
using Legion.Core.Messages.Handler;

using Module = Autofac.Module;

namespace Legion.Autofac.Modules
{
    public class ScannedMessageHandlersModule : Module
    {
        private readonly IEnumerable<Assembly> assembliesToScan;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembliesToScan">Assemblies that should be scanned to find message handlers.</param>
        public ScannedMessageHandlersModule(IEnumerable<Assembly> assembliesToScan = null)
        {
            this.assembliesToScan = assembliesToScan;
            if (this.assembliesToScan == null)
            {
                this.assembliesToScan = new[] { Assembly.GetEntryAssembly() }.Distinct();
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MessageDispatcher(c.Resolve<IMessageListener>(), c.Resolve<IMessageHandlerRegistry>()))
                   .As<IMessageDispatcher>();

            var messageHandlerRegistry = new MessageHandlerRegistry();
            builder.Register(c =>
                {
                    messageHandlerRegistry.ScanForMessageHandlerTypes(this.assembliesToScan, t => builder.RegisterType(t));
                    var componentContext = c.Resolve<IComponentContext>();
                    messageHandlerRegistry.ConnectContainerResolution(t => (IMessageHandler)componentContext.Resolve(t));
                    return messageHandlerRegistry;
                }).As<IMessageHandlerRegistry>().SingleInstance();
        }
    }
}