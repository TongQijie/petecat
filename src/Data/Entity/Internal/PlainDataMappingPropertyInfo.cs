using Petecat.Data.Attributes;
using System.Reflection;

namespace Petecat.Data.Entity
{
    internal class PlainDataMappingPropertyInfo : IDataMappingPropertyInfo
    {
        public PlainDataMappingPropertyInfo(PropertyInfo propertyInfo, PlainDataMappingAttribute dataMappingAttribute)
        {
            PropertyInfo = propertyInfo;
            DataMappingAttribute = dataMappingAttribute;
        }

        public PropertyInfo PropertyInfo { get; private set; }

        public PlainDataMappingAttribute DataMappingAttribute { get; private set; }

        public string Key { get { return DataMappingAttribute.ColumnName ?? PropertyInfo.Name; } }
    }
}
