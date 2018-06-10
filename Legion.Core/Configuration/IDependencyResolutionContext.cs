using System;
using System.Linq;

namespace Legion.Core.Configuration
{
    /// <summary>
    /// Context used to resolve dependencies of your created instances.
    /// </summary>
    public interface IDependencyResolutionContext
    {
        /// <summary>
        /// Resolve the given type and get an instance of it.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>The instance.</returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolve the given type and get an instance of it.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <returns>The instance.</returns>
        T Resolve<T>();

        /// <summary>
        /// Autofac specifica.
        /// </summary>
        /// <returns></returns>
        IDependencyResolutionContext ResolveForLater();
    }
}