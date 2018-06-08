using System.Linq;

using Autofac;

namespace Legion.Autofac.Configuration
{
    public class AutofacDependencyInjectionContext : IDependencyInjectionContext
    {
        public AutofacDependencyInjectionContext(ContainerBuilder containerBuilder)
        {
            this.ContainerBuilder = containerBuilder;
        }

        public ContainerBuilder ContainerBuilder { get; }
    }
}