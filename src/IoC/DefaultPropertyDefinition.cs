using Petecat.Extending;

using System.Reflection;

namespace Petecat.IoC
{
    public class DefaultPropertyDefinition : IPropertyDefinition
    {
        public DefaultPropertyDefinition(PropertyInfo propertyInfo)
        {
            Info = propertyInfo;
        }

        public string PropertyName { get { return Info.Name; } }

        public MemberInfo Info { get; private set; }

        public void SetValue(object instance, object value)
        {
            var propertyInfo = Info as PropertyInfo;

            object typeChangedValue;
            if (value.Convertible(propertyInfo.PropertyType, out typeChangedValue))
            {
                propertyInfo.SetValue(instance, typeChangedValue, null);
            }
            else
            {
                propertyInfo.SetValue(instance, null, null);
            }
        }
    }
}
