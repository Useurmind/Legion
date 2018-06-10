using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;

using Legion.Core.Configuration;
using Legion.Core.Messages.Handler;
using Legion.Core.Messages.Types;

namespace Legion.Autofac.Configuration
{
    public static class AutofacDependencyInjectionExtensions
    {
        /// <summary>
        /// Start to configure legion usage with autofac.
        /// </summary>
        /// <param name="containerBuilder">The autofac container builder.</param>
        /// <returns></returns>
        public static IDependencyRegistrationContext AddLegion(this ContainerBuilder containerBuilder)
        {
            return new AutofacDependencyRegistrationContext(containerBuilder);
        }
    }
}