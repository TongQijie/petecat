using System.Configuration;

namespace Petecat.Restful
{
    /// <summary>
    /// Static configuration manager.
    /// </summary>
    internal class StaticConfigurationManager : IStaticConfigurationManager
    {
        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>App setting configuration value.</returns>
        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }
    }
}
