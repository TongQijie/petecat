using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
namespace Petecat.Restful
{
    public class DefaultServicesScope : IServicesScope, IServicesLocator, IServiceProvider, IDisposable
    {
        protected Dictionary<Type, Dictionary<string, IServiceDefinition>> registeredServices = new Dictionary<Type, Dictionary<string, IServiceDefinition>>();

        protected ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> singletonServiceInstances = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>();

        protected ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> scopeServiceInstances = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>();

        protected IActivator activator = null;

        public DefaultServicesScope(IServicesDefinitionContainer servicesDefinitionContainer, IActivator activator)
        {
            this.activator = activator;
            servicesDefinitionContainer.ServicesDefinition.ForEach(delegate(IServiceDefinition serviceDefinition)
            {
                string realSubKey = string.IsNullOrEmpty(serviceDefinition.SubKey) ? string.Empty : serviceDefinition.SubKey;
                this.registeredServices.AddOrUpdate(serviceDefinition.Service, (Type type) => new Dictionary<string, IServiceDefinition>
				{
					{
						realSubKey,
						serviceDefinition
					}
				}, delegate(Type type, Dictionary<string, IServiceDefinition> old)
                {
                    old.AddOrUpdate(realSubKey, serviceDefinition, (string k, IServiceDefinition o) => serviceDefinition);
                    return old;
                });
            });
        }

        public DefaultServicesScope(Dictionary<Type, Dictionary<string, IServiceDefinition>> registeredServices, ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> singletonServiceInstances, IActivator activator)
        {
            this.registeredServices = registeredServices;
            this.singletonServiceInstances = singletonServiceInstances;
            this.activator = activator;
        }

        public bool ContainService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return this.registeredServices.ContainsKey(serviceType);
        }

        public bool ContainService<TService>()
        {
            return this.ContainService(typeof(TService));
        }

        public bool ContainService(Type serviceType, string subKey)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return this.registeredServices[serviceType].ContainsKey(subKey);
        }

        public bool ContainService<TService>(string subKey)
        {
            return this.ContainService(typeof(TService), subKey);
        }

        public TService Resolve<TService>()
        {
            TService result = default(TService);
            if (this.ContainService<TService>())
            {
                bool resolvedService = false;
                if (this.ContainService<TService>(string.Empty))
                {
                    resolvedService = true;
                    result = this.Resolve<TService>(string.Empty);
                }
                if (!resolvedService)
                {
                    IEnumerable<TService> allServices = this.ResolveAll<TService>();
                    if (!allServices.IsNullOrEmpty<TService>())
                    {
                        result = allServices.Last<TService>();
                    }
                }
            }
            return result;
        }

        public IEnumerable<TService> ResolveAll<TService>()
        {
            Func<TService, bool> func = null;
            Type type = typeof(TService);
            IEnumerable<TService> result;
            if (this.registeredServices.ContainsKey(type))
            {
                IEnumerable<TService> arg_5D_0 = this.registeredServices[type].Select(delegate(KeyValuePair<string, IServiceDefinition> sub)
                {
                    TService item = default(TService);
                    object instance = this.GetServiceInstance(sub.Value);
                    if (instance != null && instance is TService)
                    {
                        item = (TService)((object)instance);
                    }
                    return item;
                });
                if (func == null)
                {
                    func = ((TService item) => item != null);
                }
                result = arg_5D_0.Where(func).ToArray<TService>();
            }
            else
            {
                result = Enumerable.Empty<TService>();
            }
            return result;
        }

        public TService Resolve<TService>(string subKey)
        {
            TService result = default(TService);
            object instance = this.GetService(typeof(TService), subKey);
            if (instance != null && instance is TService)
            {
                result = (TService)((object)instance);
            }
            return result;
        }

        public TService GetInstance<TService>() where TService : class
        {
            return this.Resolve<TService>();
        }

        public object GetService(Type serviceType, string subKey)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            object result = null;
            IServiceDefinition serviceDefinition = this.GetServiceDefinition(serviceType, subKey);
            if (serviceDefinition != null)
            {
                result = this.GetServiceInstance(serviceDefinition);
            }
            return result;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            object result = null;
            if (this.ContainService(serviceType))
            {
                bool resolvedService = false;
                if (this.ContainService(serviceType, string.Empty))
                {
                    resolvedService = true;
                    result = this.GetService(serviceType, string.Empty);
                }
                if (!resolvedService)
                {
                    IEnumerable<object> allServices = this.GetAllServices(serviceType) as IEnumerable<object>;
                    if (!allServices.IsNullOrEmpty<object>())
                    {
                        result = allServices.Last<object>();
                    }
                }
            }
            return result;
        }

        public object GetAllServices(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            object result = null;
            if (this.registeredServices.ContainsKey(serviceType))
            {
                result = (from sub in this.registeredServices[serviceType]
                          select this.GetServiceInstance(sub.Value)).ToArray<object>();
            }
            return result;
        }

        public void Dispose()
        {
            this.scopeServiceInstances.Clear();
        }

        protected virtual object CreateServiceInstance(IServiceDefinition serviceDefinition)
        {
            object result;
            if (serviceDefinition.ServiceFactory != null)
            {
                result = serviceDefinition.ServiceFactory();
            }
            else
            {
                result = this.activator.CreateInstanceWithConstructorInjection(serviceDefinition.Implement);
            }
            return result;
        }

        protected virtual IServiceDefinition GetServiceDefinition(Type serviceType, string subkey)
        {
            IServiceDefinition result = null;
            Dictionary<string, IServiceDefinition> subDictionary;
            if (this.registeredServices.TryGetValue(serviceType, out subDictionary))
            {
                string realSubKey = string.IsNullOrEmpty(subkey) ? string.Empty : subkey;
                subDictionary.TryGetValue(realSubKey, out result);
            }
            return result;
        }

        protected virtual object GetServiceInstance(IServiceDefinition serviceDefinition)
        {
            object result = null;
            switch (serviceDefinition.LifeTime)
            {
                case ServiceLifeTime.Scope:
                    result = this.GetOrGenerateServiceInstance(this.scopeServiceInstances, serviceDefinition);
                    break;
                case ServiceLifeTime.Singleton:
                    result = this.GetOrGenerateServiceInstance(this.singletonServiceInstances, serviceDefinition);
                    break;
                case ServiceLifeTime.Transient:
                    result = this.CreateServiceInstance(serviceDefinition);
                    break;
            }
            return result;
        }

        protected virtual object GetOrGenerateServiceInstance(ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> instanceContainer, IServiceDefinition serviceDefinition)
        {
            string realSubKey = string.IsNullOrEmpty(serviceDefinition.SubKey) ? string.Empty : serviceDefinition.SubKey;
            return instanceContainer.GetOrAdd(serviceDefinition.Service, delegate(Type type)
            {
                ConcurrentDictionary<string, object> subDict = new ConcurrentDictionary<string, object>();
                subDict.TryAdd(realSubKey, this.CreateServiceInstance(serviceDefinition));
                return subDict;
            }).GetOrAdd(realSubKey, (string k) => this.CreateServiceInstance(serviceDefinition));
        }
    }
}
