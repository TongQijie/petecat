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
            if (obj == null || targetType == null)
            {
                throw new ArgumentNullException("obj or targetType");
            }

            if (targetType.IsAssignableFrom(obj.GetType()))
            {
                return obj;
            }
            else if (targetType.IsEnum)
            {
                try
                {
                    return obj.ToString().ToEnum(targetType);
                }
                catch (Exception)
                {
                    throw;
                }
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
                throw new Exception(string.Format("value {0} fails to convert into type '{1}'.", obj.ToString(), targetType.FullName));
            }
        }

        public static T ConvertTo<T>(this object obj, object defaultValue)
        {
            return (T)ConvertTo(obj, typeof(T), defaultValue);
        }

        public static object ConvertTo(this object obj, Type targetType, object defaultValue)
        {
            try
            {
                return ConvertTo(obj, targetType);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static bool Convertible<T>(this object obj, out T result)
        {
            object r;
            if (Convertible(obj, typeof(T), out r))
            {
                result = (T)r;
                return true;
            }
            else
            {
                result = typeof(T).GetDefaultValue<T>();
                return false;
            }
        }

        public static bool Convertible(this object obj, Type targetType, out object result)
        {
            try
            {
                result = ConvertTo(obj, targetType);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
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