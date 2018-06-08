using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;

using Legion.Core.Messages.Types;

using Module = Autofac.Module;

namespace Legion.Autofac.Modules
{
    public class ScannedMessagesModule : Module
    {
        private readonly IEnumerable<Assembly> assembliesToScan;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembliesToScan">Assemblies that should be scanned to find message types.</param>
        public ScannedMessagesModule(IEnumerable<Assembly> assembliesToScan = null)
        {
            this.assembliesToScan = assembliesToScan;
            if (this.assembliesToScan == null)
            {
                this.assembliesToScan = new []{ Assembly.GetEntryAssembly() }.Distinct();
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MessageTypeRegistry(this.assembliesToScan)).As<IMessageTypeRegistry>();
        }
    }
}