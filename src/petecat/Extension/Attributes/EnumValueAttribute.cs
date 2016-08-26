using System;
namespace Petecat.Extension.Attributes
{
    public class EnumValueAttribute : Attribute
    {
        public EnumValueAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
