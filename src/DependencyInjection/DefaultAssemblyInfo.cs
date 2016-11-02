using Petecat.Utility;
using Petecat.Extension;

using System.Reflection;

namespace Petecat.DependencyInjection
{
    public class DefaultAssemblyInfo : IAssemblyInfo
    {
        public DefaultAssemblyInfo(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; set; }

        public ITypeDefinition[] GetTypeDefinitions()
        {
            var typeDefinitions = new ITypeDefinition[0];

            foreach (var type in Assembly.GetTypes())
            {
                if (type.IsClass && ReflectionUtility.ContainsCustomAttribute<Attributes.DependencyInjectableAttribute>(type))
                {
                    typeDefinitions = typeDefinitions.Append(new DefaultTypeDefinition(type));
                }
            }

            return typeDefinitions;
        }
    }
}
