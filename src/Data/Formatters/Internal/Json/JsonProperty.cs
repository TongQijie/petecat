using Petecat.Collection;
using Petecat.Extension;

using System.Reflection;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal class JsonProperty : IKeyedObject<string>
    {
        public JsonProperty(PropertyInfo propertyInfo, string alias)
        {
            PropertyInfo = propertyInfo;
            Alias = alias;
            DefaultValue = propertyInfo.PropertyType.GetDefaultValue();
        }

        public string Key { get { return Alias.HasValue() ? Alias : PropertyInfo.Name; } }

        public PropertyInfo PropertyInfo { get; private set; }

        public string Alias { get; set; }

        public object DefaultValue { get; set; }
    }
}
