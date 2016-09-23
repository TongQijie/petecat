using Petecat.Collection;
using Petecat.IoC;
using Petecat.Utility;

using System;
using System.Linq;
using System.Reflection;

namespace Petecat.Service
{
    public class ServiceManager
    {
        public static ServiceManager Instance { get; set; }

        private ThreadSafeKeyedObjectCollection<string, ServiceDefinition> _LoadedServiceDefinitions = new ThreadSafeKeyedObjectCollection<string, ServiceDefinition>();

        public ServiceManager(IIoCContainer container)
        {
            _Container = container;

            LoadServicesDefinitions();
        }

        private IIoCContainer _Container = null;

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
                throw new Errors.ServiceDefinitionNotFoundException(request.ServiceName);
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
                throw new Errors.ServiceMethodNotMatchedException(request.MethodName);
            }

            if (serviceDefinition.Singleton == null)
            {
                serviceDefinition.Singleton = _Container.Resolve(serviceDefinition.ServiceType.Info as Type);
            }

            if (serviceDefinition.Singleton == null)
            {
                throw new Errors.ServiceImplementNotFoundException(serviceDefinition.Key);
            }

            object[] matchedArgumentValues;
            method.ServiceMethod.TryGetArgumentValues(methodArguments, out matchedArgumentValues);
            response.WriteObject(method.ServiceMethod.Invoke(serviceDefinition.Singleton, matchedArgumentValues));
        }

        public void InvokePost(ServiceHttpRequest request, ServiceHttpResponse response)
        {
            var serviceDefinition = _LoadedServiceDefinitions.Get(request.ServiceName, null);
            if (serviceDefinition == null)
            {
                throw new Errors.ServiceDefinitionNotFoundException(request.ServiceName);
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
                throw new Errors.ServiceMethodNotMatchedException(request.MethodName);
            }

            if (serviceDefinition.Singleton == null)
            {
                serviceDefinition.Singleton = _Container.Resolve(serviceDefinition.ServiceType.Info as Type);
            }

            if (serviceDefinition.Singleton == null)
            {
                throw new Errors.ServiceImplementNotFoundException(serviceDefinition.Key);
            }

            var requestBodyType = (method.ServiceMethod.Info as MethodInfo).GetParameters()[0].ParameterType;
            response.WriteObject(method.ServiceMethod.Invoke(serviceDefinition.Singleton, request.ReadObject(requestBodyType)));
        }
    }
}
