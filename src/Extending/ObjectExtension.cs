using System;

namespace Petecat.Extending
{
    public static class ObjectExtension
    {
        public static T ConvertTo<T>(this object obj)
        {
            return (T)ConvertTo(obj, typeof(T));
        }

        public static object ConvertTo(this object obj, Type targetType)
        {
            if (obj == null)
            {
                return targetType.GetDefaultValue();
            }

            if (targetType.IsAssignableFrom(obj.GetType()))
            {
                return obj;
            }
            else if (typeof(IConvertible).IsAssignableFrom(obj.GetType()))
            {
                try
                {
                    return Convert.ChangeType(obj, targetType);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                throw new Exception("");
            }
        }

        public static T ConvertTo<T>(this object obj, object defaultValue)
        {
            return (T)ConvertTo(obj, typeof(T), defaultValue);
        }

        public static object ConvertTo(this object obj, Type targetType, object defaultValue)
        {
            if (obj == null)
            {
                return targetType.GetDefaultValue();
            }

            if (targetType.IsAssignableFrom(obj.GetType()))
            {
                return obj;
            }
            else if (typeof(IConvertible).IsAssignableFrom(obj.GetType()))
            {
                try
                {
                    return Convert.ChangeType(obj, targetType);
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        public static bool Convertible<T>(this object obj, out object result)
        {
            return Convertible(obj, typeof(T), out result);
        }

        public static bool Convertible(this object obj, Type targetType, out object result)
        {
            result = null;
            if (obj == null)
            {
                return false;
            }

            if (targetType.IsAssignableFrom(obj.GetType()))
            {
                result = obj;
                return true;
            }
            else if (typeof(IConvertible).IsAssignableFrom(obj.GetType()))
            {
                try
                {
                    result = Convert.ChangeType(obj, targetType);
                    return true;
                }
                catch (Exception) { }
            }

            return false;
        }
    }
}