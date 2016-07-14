using Petecat.Collection;
using Petecat.IOC;
using Petecat.Utility;
using Petecat.Data.Formatters;

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Petecat.Service
{
    public class ServiceManager
    {
        public static ServiceManager Instance { get; set; }

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

            var methods = serviceDefinition.Methods.Where(x => x.MethodName.Equals(methodName, StringComparison.OrdinalIgnoreCase)).ToArray();
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

        public object Invoke(string serviceName, string methodName, Dictionary<string, object> arguments, string responseContentType)
        {
            var parameters = arguments.Select(x => new MethodArgument() { ArgumentType = x.Value.GetType(), ArgumentValue = x.Value, Name = x.Key }).ToArray();

            var serviceDefinition = _LoadedServiceDefinitions.Get(serviceName, null);
            if (serviceDefinition == null)
            {
                throw new Exception(string.Format("service named '{0}' does not exist.", serviceName));
            }

            var methods = serviceDefinition.Methods.Where(x => x.MethodName.Equals(methodName, StringComparison.OrdinalIgnoreCase)).ToArray();
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

            object[] matchedArguments;
            if (methods[0].ServiceMethod.TryGetMatchedArguments(parameters, out matchedArguments))
            {
                var response = methods[0].ServiceMethod.Invoke(serviceDefinition.Singleton, matchedArguments);

                if (responseContentType.Contains("application/xml"))
                {
                    return new XmlFormatter().WriteString(response);
                }
                else if (responseContentType.Contains("application/json"))
                {
                    return new DataContractJsonFormatter().WriteString(response);
                }
                else
                {
                    return response;
                }
            }
            else
            {
                throw new Exception("method parameter is not matched.");
            }
        }

        public object Invoke(string serviceName, string methodName, string contentType, Stream inputStream, string responseContentType)
        {
            var serviceDefinition = _LoadedServiceDefinitions.Get(serviceName, null);
            if (serviceDefinition == null)
            {
                throw new Exception(string.Format("service named '{0}' does not exist.", serviceName));
            }

            var methods = serviceDefinition.Methods.Where(x => x.MethodName.Equals(methodName, StringComparison.OrdinalIgnoreCase) 
                && (x.ServiceMethod.Info as MethodInfo).GetParameters().Length == 1).ToArray();
            if (methods.Length == 0)
            {
                var defaultMethod = serviceDefinition.Methods.FirstOrDefault(x => x.IsDefaultMethod);
                if (defaultMethod == null)
                {
                    throw new Exception(string.Format("method named '{0}' does not exist.", methodName));
                }
                else
                {
                    methods = new ServiceMethodDefinition[] { defaultMethod };
                }
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

            var requestBodyType = (methods[0].ServiceMethod.Info as MethodInfo).GetParameters()[0].GetType();

            object response = null;
            if (contentType.Contains("application/xml"))
            {
                response = methods[0].ServiceMethod.Invoke(serviceDefinition.Singleton, new XmlFormatter().ReadObject(requestBodyType, inputStream));
            }
            else if (contentType.Contains("application/json"))
            {
                response = methods[0].ServiceMethod.Invoke(serviceDefinition.Singleton, new DataContractJsonFormatter().ReadObject(requestBodyType, inputStream));
            }
            else
            {
                throw new Exception(string.Format("Request content type '{0}' not support now.", contentType));
            }

            if (responseContentType.Contains("application/xml"))
            {
                return new XmlFormatter().WriteString(response);
            }
            else if (responseContentType.Contains("application/json"))
            {
                return new DataContractJsonFormatter().WriteString(response);
            }
            else
            {
                return response;
            }
        }
    }
}
