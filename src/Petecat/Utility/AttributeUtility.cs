using System;
using System.Linq;

namespace Petecat.Utility
{
    public static class AttributeUtility
    {
        public static TAttribute GetCustomAttribute<TAttribute>(Type targetType) where TAttribute : class
        {
            var attributes = targetType.GetCustomAttributes(typeof(TAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                return attributes.FirstOrDefault() as TAttribute;
            }
            else
            {
                return null;
            }
        }

        public static bool TryGetCustomAttribute<TAttribute>(Type targetType, Predicate<TAttribute> predicate) where TAttribute : class
        {
            var attribute = GetCustomAttribute<TAttribute>(targetType);
            if (attribute == null)
            {
                return false;
            }

            return predicate(attribute);
        }
    }
}
