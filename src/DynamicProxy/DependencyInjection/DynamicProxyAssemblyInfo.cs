using Petecat.Utility;
using Petecat.Extension;
using Petecat.DependencyInjection;
using Petecat.DynamicProxy.Attributes;

using System.Reflection;

namespace Petecat.DynamicProxy.DependencyInjection
{
    public class DynamicProxyAssemblyInfo : AssemblyInfoBase
    {
        public DynamicProxyAssemblyInfo(Assembly assembly)
            : base(assembly)
        {
        }

        public override ITypeDefinition[] GetTypeDefinitions()
        {
            var typeDefinitions = new ITypeDefinition[0];

            foreach (var type in Assembly.GetTypes().Where(x => x.IsClass))
            {
                DynamicProxyInjectableAttribute attribute;
                if (Reflector.TryGetCustomAttribute(type, null, out attribute))
                {
                    if (attribute.TypeMatch && !attribute.GetType().Equals(typeof(DynamicProxyInjectableAttribute)))
                    {
                        continue;
                    }

                    var proxyType = DependencyInjector.GetObject<IDynamicProxyGenerator>().CreateProxyType(type);
                    if (proxyType == null)
                    {
                        continue;
                    }

                    if (attribute.GetType().Equals(typeof(DynamicProxyInjectableAttribute)) || !attribute.OverridedInference)
                    {
                        typeDefinitions = typeDefinitions.Append(new TypeDefinitionBase(proxyType, attribute.Inference, attribute.Sington, this));
                    }
                    else
                    {
                        typeDefinitions = typeDefinitions.Append(new TypeDefinitionBase(proxyType, null, attribute.Sington, this));
                    }
                }
            }

            return typeDefinitions;
        }
    }
}