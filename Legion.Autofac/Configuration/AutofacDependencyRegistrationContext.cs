using System;
using System.Linq;

using Autofac;

using Legion.Core.Configuration;

namespace Legion.Autofac.Configuration
{
    /// <summary>
    /// Implementation of <see cref="IDependencyResolutionContext"/> with autofac.
    /// </summary>
    public class AutofacDependencyResolutionContext : IDependencyResolutionContext
    {
        private readonly IComponentContext componentContext;

        public AutofacDependencyResolutionContext(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }

        /// <inheritdoc />
        public object Resolve(Type type)
        {
            return this.componentContext.Resolve(type);
        }

        /// <inheritdoc />
        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }

        /// <inheritdoc />
        public IDependencyResolutionContext ResolveForLater()
        {
            return new AutofacDependencyResolutionContext(this.Resolve<IComponentContext>());
        }
    }

    public class AutofacDependencyRegistrationContext : IDependencyRegistrationContext
    {
        public AutofacDependencyRegistrationContext(ContainerBuilder containerBuilder)
        {
            this.ContainerBuilder = containerBuilder;
        }

        public ContainerBuilder ContainerBuilder { get; }

        /// <inheritdoc />
        public void RegisterTransient(Func<IDependencyResolutionContext, object> createInstance, params Type[] interfaceTypes)
        {
            this.ContainerBuilder.Register(c => createInstance(new AutofacDependencyResolutionContext(c))).As(interfaceTypes);
        }

        /// <inheritdoc />
        public void RegisterTransient(Type concreteType, params Type[] interfaceTypes)
        {
            this.ContainerBuilder.RegisterType(concreteType).As(interfaceTypes);
        }

        /// <inheritdoc />
        public void RegisterSingleton(Func<IDependencyResolutionContext, object> createInstance, params Type[] interfaceTypes)
        {
            this.ContainerBuilder.Register(c => createInstance(new AutofacDependencyResolutionContext(c))).As(interfaceTypes).SingleInstance();
        }

        /// <inheritdoc />
        public void RegisterSingleton(Type concreteType, params Type[] interfaceTypes)
        {
            this.ContainerBuilder.RegisterType(concreteType).As(interfaceTypes).SingleInstance();
        }
    }
}