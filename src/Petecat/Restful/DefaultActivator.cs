using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Restful
{
    /// <summary>
    /// Default activator.
    /// </summary>
    [AutoSetupService(typeof(IActivator))]
    internal class DefaultActivator : IActivator
    {
        /// <summary>
        /// My registered services data.
        /// </summary>
        private Lazy<ConcurrentDictionary<Type, Type[]>> constructorParametersTypes = new Lazy<ConcurrentDictionary<Type, Type[]>>();

        /// <summary>
        /// Create instance with constructor injection.
        /// </summary>
        /// <param name="type">Requested type.</param>
        /// <returns>Type instance.</returns>
        public object CreateInstanceWithConstructorInjection(Type type)
        {
            return Activator.CreateInstance(type, (from t in this.GetServiceConstructorParametersTypes(type)
                                                   select ECLibraryContainer.GetService(t)).ToArray<object>());
        }

        /// <summary>
        /// Get service constructor parameters types.
        /// </summary>
        /// <param name="type">Requested type.</param>
        /// <returns>Constructor parameters types.</returns>
        private Type[] GetServiceConstructorParametersTypes(Type type)
        {
            return this.constructorParametersTypes.Value.GetOrAdd(type, new Func<Type, Type[]>(this.GenerateServiceConstructorParametersTypes));
        }

        /// <summary>
        /// Generate service constructor parameters types.
        /// </summary>
        /// <param name="type">Requested type.</param>
        /// <returns>Constructor parameters types.</returns>
        private Type[] GenerateServiceConstructorParametersTypes(Type type)
        {
            Type[] result = null;
            IEnumerable<ConstructorInfo> ctors = type.GetConstructors();
            ctors = from ctor in ctors
                    orderby ctor.GetParameters().Count<ParameterInfo>() descending
                    select ctor;
            foreach (ConstructorInfo ctor2 in ctors)
            {
                Type[] ctorParamsTypes = (from param in ctor2.GetParameters()
                                          select param.ParameterType).ToArray<Type>();
                if (!ctorParamsTypes.Any((Type t) => !t.IsInterface || !ECLibraryContainer.ContainService(t)))
                {
                    result = ctorParamsTypes;
                }
            }
            return result;
        }
    }
}
