using System;
using System.Configuration;

namespace Petecat.Utility
{
    public static class AppConfigUtility
    {
        public static T GetAppConfig<T>(string value)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[value], typeof(T));
        }

        public static T GetAppConfig<T>(string value, T defaultValue)
        {
            try
            {
                return GetAppConfig<T>(value);
            }
            catch (Exception) { }

            return defaultValue;
        }
    }
}
