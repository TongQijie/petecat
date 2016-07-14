using Petecat.Collection;
using Petecat.IOC;
using Petecat.Utility;

using System;
using System.Linq;

namespace Petecat.Service
{
    public class ServiceManager
    {
        private ThreadSafeKeyedObjectCollection<string, ServiceDefinition> _LoadedServiceDefinitions = new ThreadSafeKeyedObjectCollection<string, ServiceDefinition>();

        public ServiceManager(IContainer container)
        {
            _Container = container;

            LoadServicesDefinitions();
        }

        private IContainer _Container = null;

        private void LoadServicesDefinitions()
        {
            _Container.LoadedTypeDefinitions.ToList().ForEach(x =>
            {
                if (ReflectionUtility.ContainsCustomAttribute<Attributes.ServiceAttribute>(x.Info)
                    || ReflectionUtility.ContainsCustomAttribute<Attributes.AutoServiceAttribute>(x.Info))
                {
                    _LoadedServiceDefinitions.Add(new ServiceDefinition(x));
                }
            });
        }

        public object Invoke(string serviceName, string methodName, params object[] arguments)
        {
            var parameters = new MethodArgument[arguments.Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                parameters[i] = new MethodArgument()
                {
                    ArgumentType = arguments[i].GetType(),
                    ArgumentValue = arguments[i],
                    Index = i,
                };
            }

            var serviceDefinition = _LoadedServiceDefinitions.Get(serviceName, null);
            if (serviceDefinition == null)
            {
                throw new Exception(string.Format("service named '{0}' does not exist.", serviceName));
            }

            var methods = serviceDefinition.Methods.Where(x => x.MethodName == methodName).ToArray();
            if (methods.Length == 0)
            {
                var defaultMethod = serviceDefinition.Methods.FirstOrDefault(x => x.IsDefaultMethod);
                if (defaultMethod == null || !defaultMethod.ServiceMethod.IsMatch(parameters))
                {
                    throw new Exception(string.Format("method named '{0}' does not exist.", methodName));
                }
                else
                {
                    methods = new ServiceMethodDefinition[] { defaultMethod };
                }
            }
            else
            {
                methods = methods.Where(x => x.ServiceMethod.IsMatch(parameters)).ToArray();
            }

            if (methods.Length == 0)
            {
                throw new Exception(string.Format("method named '{0}' does not exist.", methodName));
            }
            
            if (methods.Length > 1)
            {
                throw new Exception(string.Format("more than one method named '{0}' exists.", methodName));
            }

            if (serviceDefinition.Singleton == null)
            {
                serviceDefinition.Singleton = _Container.AutoResolve(serviceDefinition.ServiceType.Info as Type);
            }

            return methods[0].ServiceMethod.Invoke(serviceDefinition.Singleton, arguments);
        }
    }
}
