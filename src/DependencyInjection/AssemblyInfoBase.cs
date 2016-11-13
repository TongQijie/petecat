using Petecat.Utility;
using Petecat.Extension;
using Petecat.DependencyInjection.Attribute;

using System.Reflection;

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

            foreach (var type in Assembly.GetTypes().Where(x => x.IsClass))
            {
                DependencyInjectableAttribute attribute;
                if (Reflector.TryGetCustomAttribute(type, null, out attribute))
                {
                    if (attribute.TypeMatch && !attribute.GetType().Equals(typeof(DependencyInjectableAttribute)))
                    {
                        continue;
                    }

                    if (attribute.GetType().Equals(typeof(DependencyInjectableAttribute)) || !attribute.OverridedInference)
                    {
                        typeDefinitions = typeDefinitions.Append(new TypeDefinitionBase(type, attribute.Inference, attribute.Singleton, attribute.Priority, this));
                    }
                    else
                    {
                        typeDefinitions = typeDefinitions.Append(new TypeDefinitionBase(type, null, attribute.Singleton, attribute.Priority, this));
                    }
                }
            }

            return typeDefinitions;
        }
    }
}
