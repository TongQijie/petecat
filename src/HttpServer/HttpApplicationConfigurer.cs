using Petecat.Configuring;
using Petecat.HttpServer.Configuration;
using Petecat.Extension;
using System;
using Petecat.DependencyInjection.Attributes;
namespace Petecat.HttpServer
{
    [DependencyInjectable(Inference = typeof(IHttpApplicationConfigurer), Singleton = true)]
    public class HttpApplicationConfigurer : IHttpApplicationConfigurer
    {
        private const string CacheKey = "Global_HttpApplicationConfiguration";

        private IStaticFileConfigurer _StaticFileConfigurer = null;

        public HttpApplicationConfigurer(IStaticFileConfigurer staticFileConfigurer)
        {
            _StaticFileConfigurer = staticFileConfigurer;
        }

        public string GetStaticResourceMapping(string key)
        {
            var httpApplicationConfiguration = _StaticFileConfigurer.GetValue<IHttpApplicationConfiguration>(CacheKey);
            if (httpApplicationConfiguration == null)
            {
                return null;
            }

            var config = httpApplicationConfiguration.StaticResourceMappingConfiguration.FirstOrDefault(x => string.Equals(key, x.Key, StringComparison.OrdinalIgnoreCase));
            if (config == null)
            {
                return null;
            }

            return config.Value;
        }

        public string GetHttpApplicationRouting(string key)
        {
            var httpApplicationConfiguration = _StaticFileConfigurer.GetValue<IHttpApplicationConfiguration>(CacheKey);
            if (httpApplicationConfiguration == null)
            {
                return null;
            }

            var config = httpApplicationConfiguration.HttpApplicationRoutingConfiguration.FirstOrDefault(x => string.Equals(key, x.Key, StringComparison.OrdinalIgnoreCase));
            if (config == null)
            {
                return null;
            }

            return config.Value;
        }
    }
}
