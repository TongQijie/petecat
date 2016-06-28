using System.Reflection;

namespace Petecat.Network.Shared
{
    internal class PropertyItem
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public FieldItemAttribute FieldItemAttribute { get; private set; }

        public PropertyItem(PropertyInfo propertyInfo, FieldItemAttribute fieldItemAttribute)
        {
            PropertyInfo = propertyInfo;
            FieldItemAttribute = fieldItemAttribute;
        }
    }
}
