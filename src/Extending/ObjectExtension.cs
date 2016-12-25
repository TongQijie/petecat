using System;

using Petecat.Data;
using Petecat.DependencyInjection;

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
            else if (targetType.IsEnum)
            {
                return obj.ToString().ToEnum(targetType);
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

        public static bool EqualsWith(this object obj, object another)
        {
            return (obj == null && another == null)
                || (obj != null && obj.Equals(another))
                || (another != null && another.Equals(obj));
        }

        public static object ShallowCopy(this object obj)
        {
            return DependencyInjector.GetObject<IReplicator>().ShallowCopy(obj);
        }

        public static T ShallowCopy<T>(this object obj)
        {
            return (T)obj.ShallowCopy();
        }

        public static object DeepCopy(this object obj)
        {
            return DependencyInjector.GetObject<IReplicator>().DeepCopy(obj);
        }

        public static T DeepCopy<T>(this object obj)
        {
            return (T)obj.DeepCopy();
        }
    }
}