using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.IO;

namespace Petecat.Restful
{
    /// <summary>
    /// Entry point for the container infrastructure for Newegg EC Library.
    /// </summary>
    public static class ECLibraryContainer
    {
        /// <summary>
        /// Preload service container.
        /// </summary>
        private static Dictionary<Type, object> preloadServiceContainer;

        /// <summary>
        /// Static singleton services container.
        /// </summary>
        private static IServicesContainer singletonServicesContainer;

        /// <summary>
        /// Gets or sets the current container used to resolve Entlib objects (for use by the various static factories).
        /// </summary>
        public static IServicesContainer Current
        {
            get
            {
                return ECLibraryContainer.singletonServicesContainer;
            }
            set
            {
                ECLibraryContainer.singletonServicesContainer = value;
            }
        }

        /// <summary>
        /// Initializes static members of the ECLibraryContainer class.
        /// </summary>
        static ECLibraryContainer()
        {
            ECLibraryContainer.preloadServiceContainer = new Dictionary<Type, object>();
            ECLibraryContainer.singletonServicesContainer = null;
            ECLibraryContainer.SetPreloadService();
            ECLibraryContainer.LoadAssemblies();
            ECLibraryContainer.ServiceAutoWiring();
            ECLibraryContainer.InitializeCurrentServiceLocator();
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type serviceType.-or- null if there is no service object of type serviceType.</returns>
        public static object GetService(Type serviceType)
        {
            object result = null;
            if (ECLibraryContainer.Current != null)
            {
                result = ECLibraryContainer.Current.GetService(serviceType);
            }
            if (result == null)
            {
                result = ECLibraryContainer.GetPreloadService(serviceType);
            }
            return result;
        }

        /// <summary>
        /// Get an instance of the given TService.
        /// </summary>
        /// <typeparam name="TService">Type of service requested.</typeparam>
        /// <returns>The requested service instance.</returns>
        public static TService Get<TService>() where TService : class
        {
            TService result = default(TService);
            if (ECLibraryContainer.Current != null)
            {
                result = ECLibraryContainer.Current.Resolve<TService>();
            }
            if (result == default(TService))
            {
                result = ECLibraryContainer.GetPreloadService<TService>();
            }
            return result;
        }

        /// <summary>
        /// Check whether the service has been registered in service locator.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>True if the service has been registered; otherwise false.</returns>
        public static bool ContainService(Type serviceType)
        {
            bool result = false;
            if (ECLibraryContainer.Current != null)
            {
                result = ECLibraryContainer.Current.ContainService(serviceType);
            }
            if (!result)
            {
                result = ECLibraryContainer.preloadServiceContainer.ContainsKey(serviceType);
            }
            return result;
        }

        /// <summary>
        /// Get preload service.
        /// </summary>
        /// <typeparam name="TService">Type of service.</typeparam>
        /// <returns>Service instance.</returns>
        private static TService GetPreloadService<TService>()
        {
            TService result = default(TService);
            object instance = ECLibraryContainer.GetPreloadService(typeof(TService));
            if (instance != null && instance is TService)
            {
                result = (TService)((object)instance);
            }
            return result;
        }

        /// <summary>
        /// Get preload service.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>Service instance.</returns>
        private static object GetPreloadService(Type serviceType)
        {
            object result = null;
            ECLibraryContainer.preloadServiceContainer.TryGetValue(serviceType, out result);
            return result;
        }

        /// <summary>
        /// Set preload service.
        /// </summary>
        private static void SetPreloadService()
        {
            IActivator activator = new DefaultActivator();
            ECLibraryContainer.preloadServiceContainer.Add(typeof(IActivator), activator);
            ECLibraryContainer.preloadServiceContainer.Add(typeof(IDictionaryUtility), activator.CreateInstanceWithConstructorInjection(typeof(DefaultDictionaryUtility)));
            ECLibraryContainer.preloadServiceContainer.Add(typeof(ICurrentAppDomain), activator.CreateInstanceWithConstructorInjection(typeof(CurrentAppDomain)));
            ECLibraryContainer.preloadServiceContainer.Add(typeof(IStaticAssembly), activator.CreateInstanceWithConstructorInjection(typeof(StaticAssembly)));
            ECLibraryContainer.preloadServiceContainer.Add(typeof(IStaticPath), activator.CreateInstanceWithConstructorInjection(typeof(StaticPath)));
            ECLibraryContainer.preloadServiceContainer.Add(typeof(IStaticDirectory), activator.CreateInstanceWithConstructorInjection(typeof(StaticDirectory)));
            ECLibraryContainer.preloadServiceContainer.Add(typeof(IStaticConfigurationManager), activator.CreateInstanceWithConstructorInjection(typeof(StaticConfigurationManager)));
            ECLibraryContainer.preloadServiceContainer.Add(typeof(IAssemblyUtility), activator.CreateInstanceWithConstructorInjection(typeof(AssemblyUtility)));
            ECLibraryContainer.preloadServiceContainer.Add(typeof(IAssembliesLoader), activator.CreateInstanceWithConstructorInjection(typeof(DefaultAssemblyLoader)));
            ECLibraryContainer.preloadServiceContainer.Add(typeof(IAssemblyTypeLoader), activator.CreateInstanceWithConstructorInjection(typeof(DefaultAssemblyTypeLoader)));
            ECLibraryContainer.preloadServiceContainer.Add(typeof(IServicesWirer), activator.CreateInstanceWithConstructorInjection(typeof(ServicesWirer)));
        }

        /// <summary>
        /// Load assemblies.
        /// </summary>
        private static void LoadAssemblies()
        {
            ECLibraryContainer.GetPreloadService<IAssembliesLoader>().LoadAssemblies();
        }

        /// <summary>
        /// Service auto wiring.
        /// </summary>
        private static void ServiceAutoWiring()
        {
            IServicesWirer servicesWirer = ECLibraryContainer.GetPreloadService<IServicesWirer>();
            servicesWirer.Wire();
            IServicesDefinitionContainer servicesDefinitionContainer = servicesWirer.ServicesDefinitionContainer;
            servicesDefinitionContainer.RegisterService<IServicesDefinitionContainer>(() => servicesDefinitionContainer, ServiceLifeTime.Singleton);
            ECLibraryContainer.preloadServiceContainer.Add(typeof(IServicesDefinitionContainer), servicesDefinitionContainer);
        }

        /// <summary>
        /// Initialize current service locator.
        /// </summary>
        private static void InitializeCurrentServiceLocator()
        {
            IEnumerable<Assembly> assemblies = ECLibraryContainer.Get<ICurrentAppDomain>().GetDomainAssemblies();
            IAssemblyTypeLoader typeloader = ECLibraryContainer.Get<IAssemblyTypeLoader>();
            IActivator activator = ECLibraryContainer.Get<IActivator>();
            StringBuilder messageBuilder = new StringBuilder();
            IEnumerable<Type> filtedtypes = assemblies.SelectMany(delegate(Assembly assembly)
            {
                string message;
                IEnumerable<Type> types = typeloader.GetTypes(assembly, new Func<Type, bool>(ECLibraryContainer.FilterFrameworkServiceLocator), out message);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    messageBuilder.AppendLine(message);
                }
                return types;
            }).ToArray<Type>();
            if (messageBuilder.Length > 0)
            {
                ECLibraryContainer.LogException(messageBuilder.ToString());
            }
            ECLibraryContainer.singletonServicesContainer = (from framewordServiceContainerType in filtedtypes
                                                             select activator.CreateInstanceWithConstructorInjection(framewordServiceContainerType) as IAutoSetupServicesContainer into locator
                                                             where locator != null
                                                             select locator into container
                                                             orderby container.Priority descending
                                                             select container).FirstOrDefault<IAutoSetupServicesContainer>().Resolve<IServicesContainer>();
        }

        /// <summary>
        /// Filter framework service locator.
        /// </summary>
        /// <param name="type">Type instance.</param>
        /// <returns>Whether type is framework service locator.</returns>
        private static bool FilterFrameworkServiceLocator(Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces.Any((Type itfc) => itfc == typeof(IAutoSetupServicesContainer));
        }

        /// <summary>
        /// Log message.
        /// </summary>
        /// <param name="content">Message content.</param>
        private static void LogException(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FrameworkLoadException", DateTime.Now.ToString("yyyyMMdd-HHmmss-fffffff") + "-FrameworkLocator" + ".log");
                    //fileName.EnsureFolderExist();
                    File.WriteAllText(fileName, content);
                }
                catch
                {
                }
            }
        }
    }
}
