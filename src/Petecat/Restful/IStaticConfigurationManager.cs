namespace Petecat.Restful
{
    /// <summary>
    /// Static configuration manager.
    /// </summary>
    public interface IStaticConfigurationManager
    {
        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>App setting configuration value.</returns>
        string GetAppSetting(string key);
    }
}
