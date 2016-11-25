﻿using System;
using System.Reflection;

namespace Petecat.Extending
{
    public static class TypeExtension
    {
        public static object CreateInstance(this Type source, params object[] parameters)
        {
            return Activator.CreateInstance(source, parameters);
        }

        public static T CreateInstance<T>(this Type source, params object[] parameters)
        {
            return (T)CreateInstance(source, parameters);
        }

        public static object GetDefaultValue(this Type source)
        {
            if (source.IsValueType)
            {
                return source.CreateInstance();
            }
            else
            {
                return null;
            }
        }

        public static T GetDefaultValue<T>(this Type source)
        {
            return (T)GetDefaultValue(source);
        }

        public static MethodInfo GetNonGenericMethod(this Type source, string name, Type[] parameterTypes)
        {
            foreach (var methodInfo in source.GetMethods().Subset(x => x.Name == name && !x.IsGenericMethod))
            {
                if (methodInfo.GetParameters().Select(x => x.ParameterType).EqualsWith(parameterTypes))
                {
                    return methodInfo;
                }
            }

            return null;
        }

        public static MethodInfo GetGenericMethod(this Type source, string name, Type[] parameterTypes, string[] genericArguments)
        {
            foreach (var methodInfo in source.GetMethods().Subset(x => x.Name == name && x.IsGenericMethod))
            {
                if (methodInfo.GetParameters().Select(x => x.ParameterType).EqualsWith(parameterTypes)
                    && methodInfo.GetGenericArguments().Select(x => x.Name).EqualsWith(genericArguments))
                {
                    return methodInfo;
                }
            }

            return null;
        }
    }
}