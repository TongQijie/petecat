using Petecat.EntityFramework.Attribute;
using System.Reflection;

namespace Petecat.EntityFramework.Internal
{
    internal class CompositeValuePropertyInfo
    {
        public CompositeValuePropertyInfo(PropertyInfo propertyInfo, CompositeValueAttribute attribute)
        {
            PropertyInfo = propertyInfo;
            Prefix = attribute.Prefix;
        }

        public PropertyInfo PropertyInfo { get; private set; }

        public string Prefix { get; private set; }
    }
}
