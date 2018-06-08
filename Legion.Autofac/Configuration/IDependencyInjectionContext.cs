using System.Linq;

using Autofac;

namespace Legion.Autofac.Configuration
{
    public interface IDependencyInjectionContext
    {
        ContainerBuilder ContainerBuilder { get; }
    }
}
