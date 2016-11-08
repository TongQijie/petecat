using System.Reflection;

using Petecat.Extension;

namespace Petecat.Formatter.Json
{
    internal class JsonProperty
    {
        public JsonProperty(PropertyInfo propertyInfo, string alias, bool isJsonObject)
        {
            PropertyInfo = propertyInfo;
            Alias = alias;
            DefaultValue = propertyInfo.PropertyType.GetDefaultValue();
            IsJsonObject = isJsonObject;
        }

        public string Key { get { return Alias.HasValue() ? Alias : PropertyInfo.Name; } }

        public PropertyInfo PropertyInfo { get; private set; }

        public string Alias { get; set; }

        public object DefaultValue { get; private set; }

        public bool IsJsonObject { get; set; }
    }
}
