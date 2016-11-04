using Petecat.DependencyInjection;
using Petecat.Utility;
using Petecat.Extension;
using System.Reflection;
namespace Petecat.Configuring.DependencyInjection
{
    public class StaticFileAssemblyInfo: AssemblyInfoBase
    {
        public StaticFileAssemblyInfo(Assembly assembly)
            : base(assembly)
        {
        }

        public override ITypeDefinition[] GetTypeDefinitions()
        {
            var typeDefinitions = new ITypeDefinition[0];

            foreach (var type in Assembly.GetTypes())
            {
                if (type.IsClass && Reflector.ContainsCustomAttribute<Attributes.StaticFileConfigElementAttribute>(type))
                {
                    typeDefinitions = typeDefinitions.Append(new TypeDefinitionBase(type));
                }
            }

            return typeDefinitions;
        }
    }
}
