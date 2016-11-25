using Petecat.EntityFramework.Attribute;
using System.Reflection;

using Petecat.Extending;

namespace Petecat.EntityFramework.Internal
{
    internal class SimpleValuePropertyInfo
    {
        public SimpleValuePropertyInfo(PropertyInfo propertyInfo, SimpleValueAttribute attribute)
        {
            PropertyInfo = propertyInfo;
            ColumnName = attribute.ColumnName.HasValue() ? attribute.ColumnName : PropertyInfo.Name;
        }

        public PropertyInfo PropertyInfo { get; private set; }

        public string ColumnName { get; set; }
    }
}