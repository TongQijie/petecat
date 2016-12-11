using System;
using System.Linq;
using System.Reflection;

namespace Petecat.Utility
{
    public static class Reflector
    {
        public static bool ContainsCustomAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : class
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(TAttribute), false);
            return attributes != null && attributes.Length > 0;
        }

        public static TAttribute GetCustomAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : class
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(TAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                return attributes.FirstOrDefault() as TAttribute;
            }
            else
            {
                return null;
            }
        }

        public static bool TryGetCustomAttribute<TAttribute>(MemberInfo memberInfo, Predicate<TAttribute> predicate, out TAttribute attribute) where TAttribute : class
        {
            var attr = GetCustomAttribute<TAttribute>(memberInfo);
            if (attr != null && (predicate == null || predicate(attr)))
            {
                attribute = attr;
                return true;
            }

            attribute = null;
            return false;
        }
    }
}