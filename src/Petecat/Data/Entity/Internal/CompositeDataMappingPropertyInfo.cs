using System.Reflection;

namespace Petecat.Data.Entity
{
    internal class CompositeDataMappingPropertyInfo : IDataMappingPropertyInfo
    {
        public CompositeDataMappingPropertyInfo(PropertyInfo propertyInfo, CompositeDataMappingAttribute compositeDataMappingAttribute)
        {
            PropertyInfo = propertyInfo;
            CompositeDataMappingAttribute = compositeDataMappingAttribute;
        }

        public CompositeDataMappingAttribute CompositeDataMappingAttribute { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }
        
        public string Key { get { return CompositeDataMappingAttribute.Type.FullName; } }
    }
}
