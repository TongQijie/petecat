using Petecat.Extending;
using Petecat.Collection;

using System.Reflection;

namespace Petecat.Data.Formatters.Internal.Binary
{
    internal class BinaryProperty : IKeyedObject<string>
    {
        public BinaryProperty(PropertyInfo propertyInfo, string alias, int index)
        {
            PropertyInfo = propertyInfo;
            Alias = alias;
            Index = index;
            DefaultValue = propertyInfo.PropertyType.GetDefaultValue();
        }

        public string Key { get { return Alias.HasValue() ? Alias : PropertyInfo.Name; } }

        public PropertyInfo PropertyInfo { get; private set; }

        public string Alias { get; set; }

        public object DefaultValue { get; set; }

        public int Index { get; private set; }
    }
}
