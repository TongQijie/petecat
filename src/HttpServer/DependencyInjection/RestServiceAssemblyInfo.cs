using System.Reflection;

using Petecat.Utility;
using Petecat.Extending;
using Petecat.DependencyInjection;
using Petecat.HttpServer.Attribute;

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

            foreach (var type in Assembly.GetTypes().Subset(x => x.IsClass))
            {
                RestServiceInjectableAttribute attribute;
                if (Reflector.TryGetCustomAttribute(type, x => x.GetType().Equals(typeof(RestServiceInjectableAttribute)), out attribute))
                {
                    typeDefinitions = typeDefinitions.Append(new RestServiceTypeDefinition(type, attribute.ServiceName));
                }
            }

            return typeDefinitions.Append(base.GetTypeDefinitions());
        }
    }
}
