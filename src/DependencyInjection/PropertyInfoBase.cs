using System.Reflection;

namespace Petecat.DependencyInjection
{
    public class PropertyInfoBase : IPropertyInfo
    {
        public PropertyInfoBase(ITypeDefinition typeDefinition, PropertyInfo propertyInfo)
        {
            TypeDefinition = typeDefinition;
            PropertyDefinition = new PropertyDefinitionBase(propertyInfo);
            PropertyName = propertyInfo.Name;
        }

        public ITypeDefinition TypeDefinition { get; private set; }

        public IPropertyDefinition PropertyDefinition { get; private set; }

        public string PropertyName { get; private set; }

        public object PropertyValue { get; private set; }

        public void SetValue(object value)
        {
            PropertyValue = value;
        }
    }
}
