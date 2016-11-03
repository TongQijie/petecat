using System.Reflection;

namespace Petecat.DependencyInjection
{
    public class PropertyDefinitionBase : IPropertyDefinition
    {
        public PropertyDefinitionBase(PropertyInfo propertyInfo)
        {
            Info = propertyInfo;
        }

        public MemberInfo Info { get; private set; }
    }
}
