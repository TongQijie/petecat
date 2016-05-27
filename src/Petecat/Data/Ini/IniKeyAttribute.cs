using System;

namespace Petecat.Data.Ini
{
    public class IniKeyAttribute : Attribute
    {
        public IniKeyAttribute()
        {
        }

        public IniKeyAttribute(string elementName)
            : this()
        {
            ElementName = elementName;
        }

        public IniKeyAttribute(string elementName, object defaultValue) 
            : this(elementName)
        {
            DefaultValue = defaultValue;
        }

        public string ElementName { get; set; }

        public object DefaultValue { get; set; }
    }
}
