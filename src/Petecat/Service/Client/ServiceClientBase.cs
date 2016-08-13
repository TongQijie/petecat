using Petecat.Network.Http;

using System;
using System.Net;

namespace Petecat.Service.Client
{
    public class ServiceClientBase
    {
        public ServiceClientBase(string resourceName)
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; private set; }

        public TResponse Call<TResponse>()
        {
            Configuration.ServiceResourceConfig serviceResourceConfig;
            string fullUrl = null;
            if (!ServiceResourceManager.Instance.TryGetResource(ResourceName, out serviceResourceConfig, out fullUrl))
            {
                throw new Exception(string.Format("service resource '{0}' not found.", ResourceName));
            }

            var request = new HttpClientRequest((HttpVerb)Enum.Parse(typeof(HttpVerb), serviceResourceConfig.Method, true), fullUrl);
            request.Request.ContentType = serviceResourceConfig.ContentType ?? "application/json";
            request.Request.Accept = serviceResourceConfig.Accept ?? "application/json";
            using (var response = request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.GetObject<TResponse>(HttpContentType.Json);
                }
                else
                {
                    throw new Exception(string.Format("failed to call service '{0}'. ", ResourceName));
                }
            }
        }

        public TResponse Call<TResponse>(object requestBody)
        {
            Configuration.ServiceResourceConfig serviceResourceConfig;
            string fullUrl = null;
            if (!ServiceResourceManager.Instance.TryGetResource(ResourceName, out serviceResourceConfig, out fullUrl))
            {
                throw new Exception(string.Format("service resource '{0}' not found.", ResourceName));
            }

            var request = new HttpClientRequest((HttpVerb)Enum.Parse(typeof(HttpVerb), serviceResourceConfig.Method, true), fullUrl);
            request.Request.ContentType = serviceResourceConfig.ContentType ?? "application/json";
            request.Request.Accept = serviceResourceConfig.Accept ?? "application/json";
            request.SetRequestBody(requestBody);
            using (var response = request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.GetObject<TResponse>();
                }
                else
                {
                    throw new Exception(string.Format("failed to call service '{0}'. ", ResourceName));
                }
            }
        }
    }
}
