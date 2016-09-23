using System;

namespace Petecat.Data.Attributes
{
    public class CompositeDataMappingAttribute : DataMappingAttributeBase
    {
        public CompositeDataMappingAttribute(Type type)
        {
            Type = type;
        }

        public CompositeDataMappingAttribute(Type type, string prefix, string conditionProperty) : this(type)
        {
            Prefix = prefix;
            ConditionalProperty = conditionProperty;
        }

        public Type Type { get; private set; }

        public string Prefix { get; set; }

        public string ConditionalProperty { get; set; }
    }
}