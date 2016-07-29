using System;
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

            try
            {
                var propertyValue = Convert.ChangeType(value, propertyInfo.PropertyType);
                propertyInfo.SetValue(instance, propertyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
