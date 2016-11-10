using Petecat.DependencyInjection;
using Petecat.Utility;
using System.Reflection;
using Petecat.Extension;
using Petecat.HttpServer.Attributes;

namespace Petecat.HttpServer.DependencyInjection
{
    public class RestServiceAssemblyInfo : AssemblyInfoBase
    {
        public RestServiceAssemblyInfo(Assembly assembly)
            : base(assembly)
        {
        }

        public override ITypeDefinition[] GetTypeDefinitions()
        {
            var typeDefinitions = new ITypeDefinition[0];

            foreach (var type in Assembly.GetTypes().Where(x => x.IsClass))
            {
                RestServiceInjectableAttribute attribute;
                if (Reflector.TryGetCustomAttribute(type, null, out attribute))
                {
                    if (attribute.TypeMatch && !attribute.GetType().Equals(typeof(RestServiceInjectableAttribute)))
                    {
                        continue;
                    }

                    typeDefinitions = typeDefinitions.Append(new RestServiceTypeDefinition(type, attribute.ServiceName));
                }
            }

            return typeDefinitions.Append(base.GetTypeDefinitions());
        }
    }
}
