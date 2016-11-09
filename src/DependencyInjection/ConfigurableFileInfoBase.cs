using Petecat.Formatter;
using Petecat.Extension;
using Petecat.DependencyInjection.Configuration;
using Petecat.Utility;
using System;

namespace Petecat.DependencyInjection
{
    public class ConfigurableFileInfoBase : IConfigurableFileInfo
    {
        public string Path { get; private set; }

        public IInstanceInfo[] GetInstanceInfos()
        {
            var instanceInfos = new IInstanceInfo[0];

            var container = new JsonFormatter().ReadObject<ConfigurableContainerConfiguration>(Path);
            if (container == null)
            {
                // TODO: throw
            }

            if (container.Instances != null && container.Instances.Length > 0)
            {
                foreach (var instance in container.Instances)
                {
                    if (instance.Name.HasValue() && instance.Type.HasValue())
                    {
                        Type type;
                        if (Reflector.TryGetType(instance.Type, out type))
                        {
                            instanceInfos = instanceInfos.Append(new InstanceInfoBase(instance.Name, new TypeDefinitionBase(type), instance.Singleton));
                        }
                    }
                }
            }

            return instanceInfos;
        }
    }
}
