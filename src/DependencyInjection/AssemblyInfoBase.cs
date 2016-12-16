using Petecat.Utility;
using Petecat.Extending;
using Petecat.DependencyInjection.Attribute;

using System.Reflection;

namespace Petecat.DependencyInjection
{
    public class AssemblyInfoBase<T> : IAssemblyInfo where T : DependencyInjectableAttribute
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
                T attribute;
                if (Reflector.TryGetCustomAttribute(type, x => x.GetType() == typeof(T), out attribute))
                {
                    typeDefinitions = typeDefinitions.Append(new TypeDefinitionBase(type, attribute.Inference, attribute.Singleton, attribute.Priority, this));
                }
            }

            return typeDefinitions;
        }
    }
}