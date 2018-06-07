using System.Collections.Generic;
using System.Reflection;

using Autofac;

using Legion.Core.Messages;
using Legion.Core.Messages.Consume;
using Legion.Core.Messages.Handler;
using Legion.Core.Messages.Types;

using Module = Autofac.Module;

namespace Heliu.Core.Messages
{
    public class MessageModule : Module
    {
        private readonly IEnumerable<Assembly> assembliesToScan;

        public MessageModule(IEnumerable<Assembly> assembliesToScan)
        {
            this.assembliesToScan = assembliesToScan;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MessageTypeRegistry()).As<IMessageTypeRegistry>();
            builder.Register(c => new MessageDispatcher(c.Resolve<IMessageListener>(), c.Resolve<IMessageHandlerRegistry>()))
                   .As<IMessageDispatcher>();

            var messageHandlerRegistry =  new MessageHandlerRegistry();
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