using System;
using System.Reflection;

namespace Petecat.Utility
{
    public static class ReflectionUtility
    {
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
    }
}