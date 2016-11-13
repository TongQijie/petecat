﻿using Petecat.DependencyInjection;
using Petecat.Formatter;
using Petecat.Network.Http;

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
                    return response.GetObject<TResponse>();
                }
                else
                {
                    throw new Errors.ServiceClientCallingFailedException(ResourceName, response.StatusCode.ToString(),
                        DependencyInjector.GetObject<IJsonFormatter>().WriteString(request.Body, Encoding.UTF8));
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
                    return response.GetObject<TResponse>();
                }
                else
                {
                    throw new Errors.ServiceClientCallingFailedException(ResourceName, response.StatusCode.ToString(),
                        DependencyInjector.GetObject<IJsonFormatter>().WriteString(request.Body, Encoding.UTF8));
                }
            }
        }
    }
}
