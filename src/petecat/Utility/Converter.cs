using System;
using System.Linq;
using System.Collections;

namespace Petecat.Utility
{
    public static class Converter
    {
        public static object GetDefaultValue(Type targetType)
        {
            if (targetType.IsValueType)
            {
                return Activator.CreateInstance(targetType);
            }
            else
            {
                return null;
            }
        }

        public static T GetDefaultValue<T>()
        {
            return (T)GetDefaultValue(typeof(T));
        }

        public static object Assignable(object sourceValue, Type targetType)
        {
            if (sourceValue == null)
            {
                return GetDefaultValue(targetType);
            }

            if (targetType.IsAssignableFrom(sourceValue.GetType()))
            {
                return sourceValue;
            }
            else if (targetType.IsArray && sourceValue.GetType().IsArray)
            {
                var sourceArray = sourceValue as Array;
                var targetArray = Array.CreateInstance(targetType.GetElementType(), sourceArray.Length) as Array;
                for (int i = 0; i < sourceArray.Length; i++)
                {
                    targetArray.SetValue(Assignable(sourceArray.GetValue(i), targetType.GetElementType()), i);
                }
                return targetArray;
            }
            else if (typeof(IList).IsAssignableFrom(sourceValue.GetType()) && typeof(IList).IsAssignableFrom(targetType))
            {
                var sourceList = sourceValue as IList;
                var targetList = Activator.CreateInstance(targetType) as IList;
                var elementType = targetType.GetGenericArguments().FirstOrDefault() ?? typeof(object);
                foreach (var elementValue in sourceList)
                {
                    targetList.Add(Assignable(elementValue, elementType));
                }
                return targetList;
            }
            else if (typeof(IConvertible).IsAssignableFrom(sourceValue.GetType()))
            {
                try
                {
                    return Convert.ChangeType(sourceValue, targetType);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        public static T Assignable<T>(object sourceValue)
        {
            return (T)Assignable(sourceValue, typeof(T));
        }

        public static bool TryBeAssignable(object sourceValue, Type targetType, out object targetValue)
        {
            try
            {
                targetValue = Assignable(sourceValue, targetType);
                return true;
            }
            catch (Exception) { }

            targetValue = null;
            return false;
        }

        public static bool TryBeAssignable<T>(object sourceValue, out T targetValue)
        {
            try
            {
                targetValue = Assignable<T>(sourceValue);
                return true;
            }
            catch (Exception) { }

            targetValue = GetDefaultValue<T>();
            return false;
        }
    }
}
