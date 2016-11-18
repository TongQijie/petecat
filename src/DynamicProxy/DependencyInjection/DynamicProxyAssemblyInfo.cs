using Petecat.Utility;
using Petecat.Extension;
using Petecat.DependencyInjection;
using Petecat.DynamicProxy.Attribute;

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
                if (Reflector.TryGetCustomAttribute(type, x => x.GetType().Equals(typeof(DynamicProxyInjectableAttribute)), out attribute))
                {
                    var proxyType = DependencyInjector.GetObject<IDynamicProxyGenerator>().CreateProxyType(type);
                    if (proxyType == null)
                    {
                        continue;
                    }

                    typeDefinitions = typeDefinitions.Append(new ITypeDefinition[]
                    {
                        new TypeDefinitionBase(proxyType, attribute.Inference, attribute.Singleton, attribute.Priority, this),
                        new TypeDefinitionBase(type, null, attribute.Singleton, attribute.Priority, this),
                    });
                }
            }

            return typeDefinitions;
        }
    }
}