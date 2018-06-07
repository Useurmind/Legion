using System;
using System.Collections.Generic;
using System.Text;

using Autofac;

using Legion.Core.Messages;
using Legion.Core.Messages.InMemory;

namespace Legion.Autofac
{
    /// <summary>
    /// Contains all registrations to use the in memory storage for events.
    /// </summary>
    public class InMemoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new InMemoryMessageStore()).As<InMemoryMessageStore>().SingleInstance();
            builder.Register(c => new InMemoryMessageSender(c.Resolve<InMemoryMessageStore>())).As<IMessageSender>();
            builder.Register(c => new InMemoryMessageListener(c.Resolve<InMemoryMessageStore>())).As<IMessageListener>();
        }
    }
}
