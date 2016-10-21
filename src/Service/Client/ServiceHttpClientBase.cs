using Petecat.Network.Http;
using Petecat.Data.Formatters;

using System;
using System.Net;
using System.Text;

namespace Petecat.Service.Client
{
    public class ServiceHttpClientBase
    {
        public ServiceHttpClientBase(string resourceName)
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
                throw new Errors.ServiceResourceNotFoundException(ResourceName);
            }

            var request = new HttpClientRequest((HttpVerb)Enum.Parse(typeof(HttpVerb), serviceResourceConfig.Method, true), fullUrl);
            request.Request.ContentType = serviceResourceConfig.ContentType ?? "application/json";
            request.Request.Accept = serviceResourceConfig.Accept ?? "application/json";
            using (var response = request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.GetObject<TResponse>(HttpFormatterSelector.Get(request.Request.Accept));
                }
                else
                {
                    throw new Errors.ServiceClientCallingFailedException(ResourceName, response.StatusCode.ToString(),
                        HttpFormatterSelector.Get(request.Request.ContentType).WriteString(request.Body, Encoding.UTF8));
                }
            }
        }

        public TResponse Call<TResponse>(object requestBody)
        {
            Configuration.ServiceResourceConfig serviceResourceConfig;
            string fullUrl = null;
            if (!ServiceResourceManager.Instance.TryGetResource(ResourceName, out serviceResourceConfig, out fullUrl))
            {
                throw new Errors.ServiceResourceNotFoundException(ResourceName);
            }

            var request = new HttpClientRequest((HttpVerb)Enum.Parse(typeof(HttpVerb), serviceResourceConfig.Method, true), fullUrl);
            request.Request.ContentType = serviceResourceConfig.ContentType ?? "application/json";
            request.Request.Accept = serviceResourceConfig.Accept ?? "application/json";
            request.SetRequestBody(requestBody);
            using (var response = request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.GetObject<TResponse>(HttpFormatterSelector.Get(request.Request.Accept));
                }
                else
                {
                    throw new Errors.ServiceClientCallingFailedException(ResourceName, response.StatusCode.ToString(),
                        HttpFormatterSelector.Get(request.Request.ContentType).WriteString(request.Body, Encoding.UTF8));
                }
            }
        }
    }
}
