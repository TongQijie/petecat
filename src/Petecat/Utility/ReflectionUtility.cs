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
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("target type not found. type name={0}", typeName), e);
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
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("target type not found. type name={0}", typeName), e);
                return null;
            }
        }

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

        public static bool TryGetCustomAttribute<TAttribute>(Type targetType, Predicate<TAttribute> predicate, out TAttribute attribute) where TAttribute : class
        {
            var attr = GetCustomAttribute<TAttribute>(targetType);
            if (attr != null && (predicate == null || predicate(attr)))
            {
                attribute = attr;
                return true;
            }

            attribute = null;
            return false;
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

        public static bool TryGetConstructorParameters(Type targetType, Dictionary<string, object> parameters, out object[] matchedParameters)
        {
            var constructors = targetType.GetConstructors();
            foreach (var constructor in constructors)
            {
                var constructorParameters = constructor.GetParameters();
                if ((parameters == null || parameters.Count == 0) && (constructorParameters == null || constructorParameters.Length == 0))
                {
                    matchedParameters = null;
                    return false;
                }

                if (parameters.Count == constructorParameters.Length)
                {
                    matchedParameters = new object[constructorParameters.Length];
                    var isMatched = true;
                    for (int i = 0; i < constructorParameters.Length; i++)
                    {
                        var key = parameters.Keys.FirstOrDefault(x => x.Equals(constructorParameters[i].Name, StringComparison.OrdinalIgnoreCase));
                        if (key == null)
                        {
                            isMatched = false;
                            break;
                        }

                        try
                        {
                            matchedParameters[i] = Convert.ChangeType(parameters[key], constructorParameters[i].ParameterType);
                        }
                        catch (Exception)
                        {
                            isMatched = false;
                            break;
                        }
                    }

                    if (isMatched)
                    {
                        return true;
                    }
                }
            }

            matchedParameters = null;
            return false;
        }

        internal static bool TryGetCustomAttribute<T1>(Type type, Predicate<IOC.Attributes.AutoResolvableAttribute> predicate, out IOC.Attributes.AutoResolvableAttribute attribute)
        {
            throw new NotImplementedException();
        }
    }
}