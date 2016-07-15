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
                if (ReflectionUtility.ContainsCustomAttribute<Attributes.ServiceImplementAttribute>(x.Info))
                {
                    _LoadedServiceDefinitions.Add(new ServiceDefinition(x));
                }

                if (ReflectionUtility.ContainsCustomAttribute<Attributes.ServiceInterfaceAttribute>(x.Info))
                {
                    _LoadedServiceDefinitions.Add(new ServiceDefinition(x));
                }
            });
        }

        public void InvokeGet(ServiceHttpRequest request, ServiceHttpResponse response)
        {
            var serviceDefinition = _LoadedServiceDefinitions.Get(request.ServiceName, null);
            if (serviceDefinition == null)
            {
                throw new Exception(string.Format("service named '{0}' does not exist.", request.ServiceName));
            }

            var methodArguments = request.ReadQueryString().Select(x => new MethodArgument() { Name = x.Key, ArgumentValue = x.Value }).ToArray();
            ServiceMethodDefinition method = null;
            if (string.IsNullOrEmpty(request.MethodName))
            {
                method = serviceDefinition.Methods.FirstOrDefault(x => x.IsDefaultMethod && x.ServiceMethod.IsMatch(methodArguments));
            }
            else
            {
                method = serviceDefinition.Methods.FirstOrDefault(x => x.MethodName.Equals(request.MethodName, StringComparison.OrdinalIgnoreCase) && x.ServiceMethod.IsMatch(methodArguments));
            }

            if (method == null)
            {
                throw new Exception("method does not exist or match.");
            }

            if (serviceDefinition.Singleton == null)
            {
                serviceDefinition.Singleton = _Container.AutoResolve(serviceDefinition.ServiceType.Info as Type);
            }

            if (serviceDefinition.Singleton == null)
            {
                throw new Exception("service implement does not exist.");
            }

            object[] matchedArgumentValues;
            method.ServiceMethod.TryGetMatchedArguments(methodArguments, out matchedArgumentValues);
            response.WriteObject(method.ServiceMethod.Invoke(serviceDefinition.Singleton, matchedArgumentValues));
        }

        public void InvokePost(ServiceHttpRequest request, ServiceHttpResponse response)
        {
            var serviceDefinition = _LoadedServiceDefinitions.Get(request.ServiceName, null);
            if (serviceDefinition == null)
            {
                throw new Exception(string.Format("service named '{0}' does not exist.", request.ServiceName));
            }

            ServiceMethodDefinition method = null;
            if (string.IsNullOrEmpty(request.MethodName))
            {
                method = serviceDefinition.Methods.FirstOrDefault(x => x.IsDefaultMethod
                    && (x.ServiceMethod.Info as MethodInfo).GetParameters().Length == 1);
            }
            else
            {
                method = serviceDefinition.Methods.FirstOrDefault(x => x.MethodName.Equals(request.MethodName, StringComparison.OrdinalIgnoreCase)
                    && (x.ServiceMethod.Info as MethodInfo).GetParameters().Length == 1);
            }

            if (method == null)
            {
                throw new Exception("method does not exist or match.");
            }

            if (serviceDefinition.Singleton == null)
            {
                serviceDefinition.Singleton = _Container.AutoResolve(serviceDefinition.ServiceType.Info as Type);
            }

            if (serviceDefinition.Singleton == null)
            {
                throw new Exception("service implement does not exist.");
            }

            var requestBodyType = (method.ServiceMethod.Info as MethodInfo).GetParameters()[0].ParameterType;
            response.WriteObject(method.ServiceMethod.Invoke(serviceDefinition.Singleton, request.ReadObject(requestBodyType)));
        }
    }
}
