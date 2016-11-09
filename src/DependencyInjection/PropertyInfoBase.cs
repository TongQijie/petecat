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

        public ITypeDefinition TypeDefinition { get; set; }

        public IPropertyDefinition PropertyDefinition { get; set; }

        public string PropertyName { get; set; }

        public object PropertyValue { get; set; }

        public void SetValue(object value)
        {
            PropertyValue = value;
        }
    }
}
