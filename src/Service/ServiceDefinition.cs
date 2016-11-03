using System.Linq;

using Petecat.IoC;
using Petecat.Utility;
using Petecat.Extension;
using Petecat.Collection;

namespace Petecat.Service
{
    public class ServiceDefinition : IKeyedObject<string>
    {
        public ServiceDefinition(ITypeDefinition typeDefinition)
        {
            ServiceType = typeDefinition;

            if (Reflector.ContainsCustomAttribute<Attributes.ServiceImplementAttribute>(typeDefinition.Info))
            {
                var attribute = Reflector.GetCustomAttribute<Attributes.ServiceImplementAttribute>(typeDefinition.Info);
                ServiceName = attribute.ServiceName;
            }
            else if (Reflector.ContainsCustomAttribute<Attributes.ServiceInterfaceAttribute>(typeDefinition.Info))
            {
                var attribute = Reflector.GetCustomAttribute<Attributes.ServiceInterfaceAttribute>(typeDefinition.Info);
                ServiceName = attribute.ServiceName;
            }

            if (string.IsNullOrWhiteSpace(ServiceName))
            {
                ServiceName = ServiceType.Info.Name;
            }

            Methods = new ServiceMethodDefinition[0];
            typeDefinition.InstanceMethods.ToList().ForEach(x =>
            {
                if (Reflector.ContainsCustomAttribute<Attributes.ServiceMethodAttribute>(x.Info))
                {
                    Methods = Methods.Append(new ServiceMethodDefinition(x));
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
