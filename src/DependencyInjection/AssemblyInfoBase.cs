using Petecat.Utility;
using Petecat.Extension;

using System.Reflection;
using System;

namespace Petecat.DependencyInjection
{
    public class AssemblyInfoBase : IAssemblyInfo
    {
        public AssemblyInfoBase(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; set; }

        public virtual ITypeDefinition[] GetTypeDefinitions()
        {
            var typeDefinitions = new ITypeDefinition[0];

            foreach (var type in Assembly.GetTypes())
            {
                if (type.IsClass && Reflector.ContainsCustomAttribute<Attributes.DependencyInjectableAttribute>(type))
                {
                    typeDefinitions = typeDefinitions.Append(new TypeDefinitionBase(type));
                }
            }

            return typeDefinitions;
        }
    }
}
