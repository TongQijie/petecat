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
                if (Reflector.TryGetCustomAttribute(type, x => x.GetType().Equals(typeof(DependencyInjectableAttribute)), out attribute))
                {
                    typeDefinitions = typeDefinitions.Append(new TypeDefinitionBase(type, attribute.Inference, attribute.Singleton, attribute.Priority, this));
                }
            }

            return typeDefinitions;
        }
    }
}
