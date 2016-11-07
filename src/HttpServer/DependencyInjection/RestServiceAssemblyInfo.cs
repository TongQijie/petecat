using Petecat.DependencyInjection;
using Petecat.Utility;
using System.Reflection;
using Petecat.Extension;

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

            foreach (var type in Assembly.GetTypes())
            {
                if (type.IsClass && Reflector.ContainsCustomAttribute<Attributes.RestServiceInjectableAttribute>(type))
                {
                    typeDefinitions = typeDefinitions.Append(new RestServiceTypeDefinition(type));
                }
            }

            return typeDefinitions;
        }
    }
}
