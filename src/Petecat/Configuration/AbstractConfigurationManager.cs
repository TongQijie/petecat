using System.Collections.Generic;
using System.Runtime.Caching;

namespace Petecat.Configuration
{
    public abstract class AbstractConfigurationManager : IConfigurationManager
    {
        protected ObjectCache _MemoryCache = null;

        protected Dictionary<string, object> _ConfigurationItems = null;

        public AbstractConfigurationManager(bool enableCache)
        {
            EnableCache = enableCache;
            if (EnableCache)
            {
                _MemoryCache = MemoryCache.Default;
            }
            else
            {
                _ConfigurationItems = new Dictionary<string, object>();
            }
        }

        public bool EnableCache { get; set; }

        public virtual void Set(string key, object value)
        {
            if (EnableCache)
            {
                if (_MemoryCache.Contains(key))
                {
                    _MemoryCache.Remove(key);
                }

                _MemoryCache.Add(key, value, new CacheItemPolicy());
            }
            else
            {
                if (_ConfigurationItems.ContainsKey(key))
                {
                    _ConfigurationItems.Remove(key);
                }

                _ConfigurationItems.Add(key, value);
            }
        }

        public virtual T Get<T>(string key)
        {
            if (EnableCache)
            {
                if (_MemoryCache.Contains(key))
                {
                    return (T)_MemoryCache.Get(key);
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            else
            {
                if (_ConfigurationItems.ContainsKey(key))
                {
                    return (T)_ConfigurationItems[key];
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public virtual T Get<T>(string key, T defaultValue)
        {
            if (EnableCache)
            {
                if (_MemoryCache.Contains(key))
                {
                    return (T)_MemoryCache.Get(key);
                }
                else
                {
                    return defaultValue;
                }
            }
            else
            {
                if (_ConfigurationItems.ContainsKey(key))
                {
                    return (T)_ConfigurationItems[key];
                }
                else
                {
                    return defaultValue;
                }
            }
        }
    }
}
