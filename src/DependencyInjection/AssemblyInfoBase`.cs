using System.Reflection;

using Petecat.Utility;
using Petecat.Extending;
using Petecat.DependencyInjection.Attribute;

namespace Petecat.DependencyInjection
{
    public class AssemblyInfoBase<TAttribute> : IAssemblyInfo where TAttribute : DependencyInjectableAttribute
    {
        public AssemblyInfoBase(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; set; }

        public virtual ITypeDefinition[] GetTypeDefinitions()
        {
            var typeDefinitions = new ITypeDefinition[0];

            foreach (var type in Assembly.GetTypes().Subset(x => x.IsClass))
            {
                TAttribute attribute;
                if (Reflector.TryGetCustomAttribute(type, x => x.GetType() == typeof(TAttribute), out attribute))
                {
                    typeDefinitions = typeDefinitions.Append(new TypeDefinitionBase(type, attribute.Inference, attribute.Singleton, attribute.Priority, this));
                }
            }

            return typeDefinitions;
        }
    }
}