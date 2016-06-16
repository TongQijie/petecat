using System.Runtime.Caching;

namespace Petecat.Configuration
{
    public delegate void ConfigurationItemChangedDelegate(IConfigurationManager configurationManager, string key);

    public interface IConfigurationManager
    {
        bool EnableCache { get; }

        void Set(string key, object value, CacheItemPolicy policy);

        T Get<T>(string key);

        T Get<T>(string key, T defaultValue);

        event ConfigurationItemChangedDelegate ConfigurationItemChanged;
    }
}