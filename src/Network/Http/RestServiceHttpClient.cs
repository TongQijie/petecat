using Petecat.Configuring;
using Petecat.Extending;
using Petecat.DependencyInjection;
using Petecat.Network.Http.Configuration;
using Petecat.DependencyInjection.Attribute;

using System;
using System.Net;

namespace Petecat.Network.Http
{
    [DependencyInjectable(Inference = typeof(IRestServiceHttpClient), Singleton = true)]
    public class RestServiceHttpClient : IRestServiceHttpClient
    {
        private const string CacheKey = "Global_RestServiceConfiguration";

        public TResponse Call<TResponse>(string resourceName)
        {
            return Call<TResponse>(resourceName, null);
        }

        public TResponse Call<TResponse>(string resourceName, object requestBody)
        {
            HttpVerb verb;
            string host;
            if (!TryGetResource(resourceName, out verb, out host))
            {
                // TODO: throw
            }

            return Call<TResponse>(verb, host, requestBody);
        }

        public TResponse Call<TResponse>(HttpVerb verb, string host, object requestBody)
        {
            var request = new HttpClientRequest(verb, host); 
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

            var restServiceConfiguration = DependencyInjector.GetObject<IStaticFileConfigurer>().GetValue<IRestServiceConfiguration>(CacheKey);
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
