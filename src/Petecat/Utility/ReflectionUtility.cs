using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Petecat.Utility
{
    public static class ReflectionUtility
    {
        public static bool TryGetType(string typeName, out Type targetType)
        {
            try
            {
                var type = Type.GetType(typeName);
                if (type != null)
                {
                    targetType = type;
                    return true;
                }
            }
            catch (Exception e)
            {
                Logging.LoggerManager.GetLogger().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("target type not found. type name={0}", typeName), e);
            }

            targetType = null;
            return false;
        }

        public static T GetInstance<T>(string typeName, params object[] parameters) where T : class
        {
            try
            {
                var targetType = Type.GetType(typeName);
                return Activator.CreateInstance(targetType, parameters) as T;
            }
            catch (Exception e)
            {
                Logging.LoggerManager.GetLogger().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("target type not found. type name={0}", typeName), e);
                return null;
            }
        }

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

        public static bool TryChangeType(object value, Type targetType, out object typeChangedValue)
        {
            if (targetType.IsAssignableFrom(value.GetType()))
            {
                typeChangedValue = value;
                return true;
            }
            else if (typeof(IConvertible).IsAssignableFrom(value.GetType()))
            {
                try
                {
                    typeChangedValue = Convert.ChangeType(value, targetType);
                }
                catch (Exception)
                {
                    typeChangedValue = null;
                    return false;
                }

                return true;
            }
            else if (targetType.IsArray && value.GetType().IsArray)
            {
                var array = value as Array;
                var collection = Array.CreateInstance(targetType.GetElementType(), array.Length) as Array;
                for (int i = 0; i < array.Length; i++)
                {
                    object result;
                    if (TryChangeType(array.GetValue(i), targetType.GetElementType(), out result))
                    {
                        collection.SetValue(result, i);
                    }
                    else
                    {
                        typeChangedValue = null;
                        return false;
                    }
                }

                typeChangedValue = collection;
                return true;
            }

            typeChangedValue = null;
            return false;
        }
    }
}