using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
namespace Petecat.Restful
{
    /// <summary>
    /// Default services wirer.
    /// </summary>
    [AutoSetupService(typeof(IServicesWirer))]
    internal class ServicesWirer : IServicesWirer
    {
        /// <summary>
        /// Current AppDomain.
        /// </summary>
        private readonly ICurrentAppDomain currentAppDomain;

        /// <summary>
        /// Assembly type loader.
        /// </summary>
        private readonly IAssemblyTypeLoader assemblyTypeLoader;

        /// <summary>
        /// Object activator.
        /// </summary>
        private readonly IActivator activator;

        /// <summary>
        /// My container.
        /// </summary>
        private readonly DefaultServicesDefinitionContainer myContainer = new DefaultServicesDefinitionContainer();

        /// <summary>
        /// Services wiring strategies.
        /// </summary>
        private IEnumerable<IServicesWiringStrategy> wiringStrategies;

        /// <summary>
        /// Gets services definition container.
        /// </summary>
        public IServicesDefinitionContainer ServicesDefinitionContainer
        {
            get
            {
                return this.myContainer;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ServicesWirer class.
        /// </summary>
        /// <param name="currentAppDomain">Current app domain.</param>
        /// <param name="assemblyTypeLoader">Assembly type loader.</param>
        /// <param name="activator">Object activator.</param>
        public ServicesWirer(ICurrentAppDomain currentAppDomain, IAssemblyTypeLoader assemblyTypeLoader, IActivator activator)
        {
            this.currentAppDomain = currentAppDomain;
            this.assemblyTypeLoader = assemblyTypeLoader;
            this.activator = activator;
            this.GenerateStrategy();
        }

        /// <summary>
        /// Wire the services.
        /// </summary>
        public void Wire()
        {
            if (!this.wiringStrategies.IsNullOrEmpty<IServicesWiringStrategy>())
            {
                this.wiringStrategies.ForEach(delegate(IServicesWiringStrategy strategy)
                {
                    IEnumerable<IServiceDefinition> servicesDefinition = strategy.GetServicesDefinition();
                    if (!servicesDefinition.IsNullOrEmpty<IServiceDefinition>())
                    {
                        servicesDefinition.ForEach(delegate(IServiceDefinition serviceDefinition)
                        {
                            if (serviceDefinition.ServiceFactory != null)
                            {
                                this.myContainer.RegisterService(serviceDefinition.Service, serviceDefinition.ServiceFactory, serviceDefinition.SubKey, serviceDefinition.LifeTime);
                            }
                            else
                            {
                                this.myContainer.RegisterService(serviceDefinition.Service, serviceDefinition.Implement, serviceDefinition.SubKey, serviceDefinition.LifeTime);
                            }
                        });
                    }
                });
            }
        }

        /// <summary>
        /// Gets services definitions.
        /// </summary>
        private void GenerateStrategy()
        {
            IEnumerable<Assembly> assemblies = this.currentAppDomain.GetDomainAssemblies();
            this.wiringStrategies = (from type in assemblies.SelectMany((Assembly assembly) => this.assemblyTypeLoader.GetTypes(assembly, new Func<Type, bool>(this.IsServicesWiringStrategy)))
                                     select this.activator.CreateInstanceWithConstructorInjection(type) as IServicesWiringStrategy).ToArray<IServicesWiringStrategy>();
            this.wiringStrategies = (from strategy in this.wiringStrategies
                                     orderby strategy.Priority
                                     select strategy).ToArray<IServicesWiringStrategy>();
        }

        /// <summary>
        /// Check whether type is an instance services module..
        /// </summary>
        /// <param name="type">Current type.</param>
        /// <returns>True is an instance services module. False is not.</returns>
        private bool IsServicesWiringStrategy(Type type)
        {
            bool result;
            if (!type.IsClass)
            {
                result = false;
            }
            else if (type.IsAbstract)
            {
                result = false;
            }
            else
            {
                result = type.GetTypeInfo().ImplementedInterfaces.Any((Type itfc) => itfc == typeof(IServicesWiringStrategy));
            }
            return result;
        }
    }
}
