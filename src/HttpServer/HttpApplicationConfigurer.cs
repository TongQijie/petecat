using Petecat.Configuring;
using Petecat.HttpServer.Configuration;
using Petecat.Extension;
using System;
namespace Petecat.HttpServer
{
    public static class HttpApplicationConfigurer
    {
        public static string GetStaticResourceMapping(string key)
        {
            var httpApplicationConfig = ConfigurationFactory.GetManager().GetValue<HttpApplicationConfiguration>("Global_HttpApplication");
            if (httpApplicationConfig == null
                || httpApplicationConfig.StaticResourceMappingConfiguration == null
                || httpApplicationConfig.StaticResourceMappingConfiguration.Length == 0)
            {
                return null;
            }

            var config = httpApplicationConfig.StaticResourceMappingConfiguration.FirstOrDefault(x => string.Equals(key, x.Key, StringComparison.OrdinalIgnoreCase));
            if (config == null)
            {
                return null;
            }

            return config.Value;
        }

        public static string GetHttpApplicationRouting(string key)
        {
            var httpApplicationConfig = ConfigurationFactory.GetManager().GetValue<HttpApplicationConfiguration>("Global_HttpApplication");
            if (httpApplicationConfig == null
                || httpApplicationConfig.HttpApplicationRoutingConfiguration == null
                || httpApplicationConfig.HttpApplicationRoutingConfiguration.Length == 0)
            {
                return null;
            }

            var config = httpApplicationConfig.HttpApplicationRoutingConfiguration.FirstOrDefault(x => string.Equals(key, x.Key, StringComparison.OrdinalIgnoreCase));
            if (config == null)
            {
                return null;
            }

            return config.Value;
        }
    }
}
