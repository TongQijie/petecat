using System;
using System.Net;
using System.Collections.Generic;

using Petecat.Extending;
using Petecat.Configuring;
using Petecat.DependencyInjection;
using Petecat.Network.Http.Configuration;
using Petecat.DependencyInjection.Attribute;

namespace Petecat.Network.Http
{
    [DependencyInjectable(Inference = typeof(IRestServiceAgency), Singleton = true)]
    public class RestServiceAgency : IRestServiceAgency
    {
        private IStaticFileConfigurer _StaticFileConfigurer;

        public RestServiceAgency(IStaticFileConfigurer staticFileConfigurer)
        {
            _StaticFileConfigurer = staticFileConfigurer;
        }

        public TResponse Call<TResponse>(string resourceName, Dictionary<string, string> queryString)
        {
            return Call<TResponse>(resourceName, null, queryString);
        }

        public TResponse Call<TResponse>(string resourceName, object requestBody, Dictionary<string, string> queryString)
        {
            HttpVerb verb;
            string host;
            if (!TryGetResource(resourceName, out verb, out host))
            {
                throw new Exception(string.Format("resource '{0}' cannot be found.", resourceName));
            }

            return Call<TResponse>(verb, host, requestBody, queryString);
        }

        public TResponse Call<TResponse>(HttpVerb verb, string host, object requestBody, Dictionary<string, string> queryString)
        {
            var request = new HttpRequest(verb, host, queryString); 
            request.Request.ContentType = "application/json";
            request.Request.Accept = "application/json";
            if (requestBody != null)
            {
                request.SetRequestBody(requestBody);
            }
            
            using (var response = request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.GetObject<TResponse>();
                }
                else
                {
                    // TODO: throw
                }
            }

            return default(TResponse);
        }

        private bool TryGetResource(string resourceName, out HttpVerb verb, out string host)
        {
            verb = HttpVerb.GET;
            host = null;

            var configurer = _StaticFileConfigurer ?? DependencyInjector.GetObject<IStaticFileConfigurer>();

            var restServiceConfiguration = configurer.GetValue<IRestServiceConfiguration>();
            if (restServiceConfiguration == null)
            {
                return false;
            }

            if (restServiceConfiguration.ResourceConfiguration == null || restServiceConfiguration.ResourceConfiguration.Length == 0)
            {
                return false;
            }

            var resource = restServiceConfiguration.ResourceConfiguration.FirstOrDefault(x => string.Equals(x.Name, resourceName, StringComparison.OrdinalIgnoreCase));
            if (resource == null)
            {
                return false;
            }

            if(!Enum.TryParse(resource.Method, true, out verb))
            {
                return false;
            }

            if (!resource.Host.HasValue())
            {
                return false;
            }

            host = resource.Host;

            return true;
        }
    }
}
