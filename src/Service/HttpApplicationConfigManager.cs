using Petecat.Configuring;
using Petecat.Service.Configuration;
using Petecat.Extension;
using System;
namespace Petecat.Service
{
    public class HttpApplicationConfigManager
    {
        private static HttpApplicationConfigManager _Instance = null;

        public static HttpApplicationConfigManager Instance { get { return _Instance ?? (_Instance = new HttpApplicationConfigManager()); } }

        public string GetStaticResourceContentMapping(string key)
        {
            var httpApplicationConfig = ConfigurationFactory.GetManager().GetValue<HttpApplicationConfig>("Global_HttpApplication");
            if (httpApplicationConfig == null 
                || httpApplicationConfig.StaticResourceContentMapping == null
                || httpApplicationConfig.StaticResourceContentMapping.KeyValues == null
                || httpApplicationConfig.StaticResourceContentMapping.KeyValues.Length == 0)
            {
                return null;
            }

            var contentMapping = httpApplicationConfig.StaticResourceContentMapping.KeyValues.FirstOrDefault(x => string.Equals(key, x.Key, StringComparison.OrdinalIgnoreCase));
            if (contentMapping == null)
            {
                return null;
            }

            return contentMapping.Value;
        }

        public string GetHttpRouting(string key)
        {
            var httpApplicationConfig = ConfigurationFactory.GetManager().GetValue<HttpApplicationConfig>("Global_HttpApplication");
            if (httpApplicationConfig == null
                || httpApplicationConfig.StaticResourceContentMapping == null
                || httpApplicationConfig.StaticResourceContentMapping.KeyValues == null
                || httpApplicationConfig.StaticResourceContentMapping.KeyValues.Length == 0)
            {
                return null;
            }

            var httpRouting = httpApplicationConfig.HttpRoutings.KeyValues.FirstOrDefault(x => string.Equals(key, x.Key, StringComparison.OrdinalIgnoreCase));
            if (httpRouting == null)
            {
                return null;
            }

            return httpRouting.Value;
        }
    }
}
