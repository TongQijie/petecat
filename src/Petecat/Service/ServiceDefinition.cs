using System;
using System.Linq;

using Petecat.Collection;
using Petecat.Utility;
using Petecat.IOC;

namespace Petecat.Service
{
    public class ServiceDefinition : IKeyedObject<string>
    {
        public ServiceDefinition(ITypeDefinition typeDefinition)
        {
            ServiceType = typeDefinition;

            if (ReflectionUtility.ContainsCustomAttribute<Attributes.ServiceAttribute>(typeDefinition.Info))
            {
                var attribute = ReflectionUtility.GetCustomAttribute<Attributes.ServiceAttribute>(typeDefinition.Info);
                ServiceName = attribute.ServiceName;
            }
            else if (ReflectionUtility.ContainsCustomAttribute<Attributes.AutoServiceAttribute>(typeDefinition.Info))
            {
                var attribute = ReflectionUtility.GetCustomAttribute<Attributes.AutoServiceAttribute>(typeDefinition.Info);
                ServiceName = attribute.ServiceName;
            }

            if (string.IsNullOrWhiteSpace(ServiceName))
            {
                ServiceName = ServiceType.Info.Name;
            }

            Methods = new ServiceMethodDefinition[0];
            typeDefinition.InstanceMethods.ToList().ForEach(x =>
            {
                if (ReflectionUtility.ContainsCustomAttribute<Attributes.ServiceMethodAttribute>(x.Info))
                {
                    Methods = Methods.Concat(new ServiceMethodDefinition[] { new ServiceMethodDefinition(x) }).ToArray();
                }
            });
        }

        public string Key { get { return ServiceName; } }

        public object Singleton { get; set; }

        public ITypeDefinition ServiceType { get; private set; }

        public string ServiceName { get; private set; }

        public ServiceMethodDefinition[] Methods { get; set; }
    }
}
