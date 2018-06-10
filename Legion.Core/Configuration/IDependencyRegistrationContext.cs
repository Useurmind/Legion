using System;
using System.Linq;

namespace Legion.Core.Configuration
{
    /// <summary>
    /// Context used to register stuff in the container.
    /// </summary>
    public interface IDependencyRegistrationContext
    {
        /// <summary>
        /// Register a factory function under a set of interfaces/types.
        /// Each call to resolve will create a new instance.
        /// </summary>
        /// <param name="createInstance">The factory function to register.</param>
        /// <param name="interfaceTypes">The types under which the created instances should be resolvable.</param>
        void RegisterTransient(Func<IDependencyResolutionContext, object> createInstance, params Type[] interfaceTypes);

        /// <summary>
        /// Register a type under a set of interfaces/types.
        /// Each call to resolve will create a new instance.
        /// </summary>
        /// <param name="concreteType">The type to register and create.</param>
        /// <param name="interfaceTypes">The types under which the created instances should be resolvable.</param>
        void RegisterTransient(Type concreteType, params Type[] interfaceTypes);

        /// <summary>
        /// Register a factory function under a set of interfaces/types.
        /// Each call to resolve will return the same instance.
        /// </summary>
        /// <param name="createInstance">The factory function to register.</param>
        /// <param name="interfaceTypes">The types under which the created instances should be resolvable.</param>
        void RegisterSingleton(Func<IDependencyResolutionContext, object> createInstance, params Type[] interfaceTypes);

        /// <summary>
        /// Register a type under a set of interfaces/types.
        /// Each call to resolve will return the same instance.
        /// </summary>
        /// <param name="concreteType">The type to register and create.</param>
        /// <param name="interfaceTypes">The types under which the created instances should be resolvable.</param>
        void RegisterSingleton(Type concreteType, params Type[] interfaceTypes);
    }
}
