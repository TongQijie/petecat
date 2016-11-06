using Petecat.Extension.Attributes;
using Petecat.Utility;

using System;
using System.Linq;
using System.Reflection;

namespace Petecat.Extension
{
    public static class TypeExtension
    {
        public static object GetDefaultValue(this Type sourceType)
        {
            if (sourceType.IsValueType)
            {
                return Activator.CreateInstance(sourceType);
            }
            else
            {
                return null;
            }
        }

        public static Enum GetEnumByValue(this Type sourceType, string enumValue)
        {
            if (sourceType == null || !sourceType.IsEnum)
            {
                return null;
            }

            var fields = sourceType.GetFields();
            foreach (var field in fields)
            {
                EnumValueAttribute attribute;
                if (Reflector.TryGetCustomAttribute<EnumValueAttribute>(field, x => x.Value.Equals(enumValue, StringComparison.OrdinalIgnoreCase), out attribute)
                    || field.Name.Equals(enumValue, StringComparison.OrdinalIgnoreCase))
                {
                    return (Enum)Enum.Parse(sourceType, field.Name);
                }
            }

            return (Enum)sourceType.GetDefaultValue();
        }

        public static string GetValueByEnum(this Type sourceType, Enum enumInstance)
        {
            if (sourceType == null || !sourceType.IsEnum)
            {
                return null;
            }

            var field = sourceType.GetFields().First(x => x.Name == enumInstance.ToString());
            EnumValueAttribute attribute;
            if (Reflector.TryGetCustomAttribute<EnumValueAttribute>(field, x => !string.IsNullOrEmpty(x.Value), out attribute))
            {
                return attribute.Value;
            }
            else
            {
                return field.Name;
            }
        }

        public static MethodInfo GetNonGenericMethod(this Type sourceType, string name, Type[] parameterTypes)
        {
            foreach (var methodInfo in sourceType.GetMethods().Where(x => x.Name == name && !x.IsGenericMethod))
            {
                if (methodInfo.GetParameters().Select(x => x.ParameterType).ToArray().EqualsWith(parameterTypes))
                {
                    return methodInfo;
                }
            }

            return null;
        }

        public static MethodInfo GetGenericMethod(this Type sourceType, string name, Type[] parameterTypes, string[] genericArguments)
        {
            foreach (var methodInfo in sourceType.GetMethods().Where(x => x.Name == name && x.IsGenericMethod))
            {
                if (methodInfo.GetParameters().Select(x => x.ParameterType).ToArray().EqualsWith(parameterTypes)
                    && methodInfo.GetGenericArguments().Select(x => x.Name).ToArray().EqualsWith(genericArguments))
                {
                    return methodInfo;
                }
            }

            return null;
        }
    }
}
