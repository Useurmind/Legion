using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;

using Module = Autofac.Module;

namespace Legion.Autofac.Modules
{
    public class SenderModule : Module
    {
        private readonly IEnumerable<Assembly> assembliesToScan;

        public SenderModule()
        {
            this.assembliesToScan = this.assembliesToScan;
            if (this.assembliesToScan == null)
            {
                this.assembliesToScan = new[] { Assembly.GetEntryAssembly(), Assembly.GetCallingAssembly() }.Distinct();
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
          
        }
    }
}