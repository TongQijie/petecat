using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Petecat.Extending;
using Petecat.Configuring;
using Petecat.HttpServer.Configuration;
using Petecat.DependencyInjection.Attribute;

namespace Petecat.HttpServer
{
    [DependencyInjectable(Inference = typeof(IHttpApplicationConfigurer), Singleton = true)]
    public class HttpApplicationConfigurer : IHttpApplicationConfigurer
    {
        private IStaticFileConfigurer _StaticFileConfigurer = null;

        public HttpApplicationConfigurer(IStaticFileConfigurer staticFileConfigurer)
        {
            _StaticFileConfigurer = staticFileConfigurer;
        }

        public string GetStaticResourceMapping(string key)
        {
            var httpApplicationConfiguration = _StaticFileConfigurer.GetValue<IHttpApplicationConfiguration>();
            if (httpApplicationConfiguration == null)
            {
                return null;
            }

            if (httpApplicationConfiguration.StaticResourceMappingConfiguration == null
                || httpApplicationConfiguration.StaticResourceMappingConfiguration.Length == 0)
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
            var httpApplicationConfiguration = _StaticFileConfigurer.GetValue<IHttpApplicationConfiguration>();
            if (httpApplicationConfiguration == null)
            {
                return null;
            }

            if (httpApplicationConfiguration.HttpApplicationRoutingConfiguration == null
                || httpApplicationConfiguration.HttpApplicationRoutingConfiguration.Length == 0)
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

        public string ApplyRewriteRule(string url)
        {
            var httpApplicationConfiguration = _StaticFileConfigurer.GetValue<IHttpApplicationConfiguration>();
            if (httpApplicationConfiguration == null)
            {
                return url;
            }

            if (httpApplicationConfiguration.RewriteRules == null
                || httpApplicationConfiguration.RewriteRules.Length == 0)
            {
                return url;
            }

            foreach (var rewriteRule in httpApplicationConfiguration.RewriteRules)
            {
                if (Regex.IsMatch(url, rewriteRule.Pattern, RegexOptions.IgnoreCase))
                {
                    if (rewriteRule.Mode == RewriteRuleMode.Override)
                    {
                        return rewriteRule.Value;
                    }
                    else if (rewriteRule.Mode == RewriteRuleMode.Replace)
                    {
                        return Regex.Replace(url, rewriteRule.Pattern, rewriteRule.Value);
                    }
                }
            }

            return url;
        }

        public Dictionary<string, string> GetReponseHeaders()
        {
            var headers = new Dictionary<string, string>();

            var httpApplicationConfiguration = _StaticFileConfigurer.GetValue<IHttpApplicationConfiguration>();
            if (httpApplicationConfiguration == null)
            {
                return headers;
            }

            if (httpApplicationConfiguration.ResponseHeaders == null
                || httpApplicationConfiguration.ResponseHeaders.Length == 0)
            {
                return headers;
            }

            foreach (var header in httpApplicationConfiguration.ResponseHeaders)
            {
                headers.Add(header.Key, header.Value);
            }

            return headers;
        }

        public string ApplyHttpRedirect(string url)
        {
            var httpApplicationConfiguration = _StaticFileConfigurer.GetValue<IHttpApplicationConfiguration>();
            if (httpApplicationConfiguration == null)
            {
                return null;
            }

            if (httpApplicationConfiguration.HttpRedirects == null
                || httpApplicationConfiguration.HttpRedirects.Length == 0)
            {
                return null;
            }

            foreach (var httpRedirect in httpApplicationConfiguration.HttpRedirects)
            {
                if (Regex.IsMatch(url, httpRedirect.Pattern, RegexOptions.IgnoreCase))
                {
                    if (httpRedirect.Mode == HttpRedirectMode.Override)
                    {
                        return httpRedirect.Redirect;
                    }
                    else if (httpRedirect.Mode == HttpRedirectMode.Replace)
                    {
                        return Regex.Replace(url, httpRedirect.Pattern, httpRedirect.Redirect);
                    }
                }
            }

            return null;
        }
    }
}
