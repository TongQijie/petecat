﻿using Petecat.Logging;
using Petecat.Extending;
using Petecat.DependencyInjection;
using Petecat.HttpServer.DependencyInjection;

using System;
using System.Web;
using System.Linq;

namespace Petecat.HttpServer
{
    public class RestServiceHttpHandler : IHttpHandler
    {
        public RestServiceHttpHandler(string serviceName, string methodName)
        {
            ServiceName = serviceName;
            MethodName = methodName;
        }

        public bool IsReusable { get { return true; } }

        public string ServiceName { get; private set; }

        public string MethodName { get; private set; }

        [ThreadStatic]
        public static RestServiceHttpRequest Request;

        [ThreadStatic]
        public static RestServiceHttpResponse Response;

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var headers = DependencyInjector.GetObject<IHttpApplicationConfigurer>().GetReponseHeaders();
                foreach (var header in headers)
                {
                    context.Response.Headers.Add(header.Key, header.Value);
                }

                Request = new RestServiceHttpRequest(context.Request, ServiceName, MethodName);
                Response = new RestServiceHttpResponse(context.Response);

                var container = DependencyInjector.GetContainer<HttpServerAssemblyContainer>();
                if (container == null)
                {
                    throw new Exception("http server container is null.");
                }

                Execute(container);
                Response.StatusCode = 200;
            }
            catch (Exception e)
            {
                DependencyInjector.GetObject<IFileLogger>().LogEvent("RestServiceHttpHandler", Severity.Error, "failed to process restservice request.", e);
                Response.Write("error occurs when processing request.", DataFormat.Text);
                Response.StatusCode = 500;
            }
        }

        public void Execute(HttpServerAssemblyContainer container)
        {
            var typeDefinition = container.RegisteredTypes.Values.OfType<RestServiceTypeDefinition>()
                .FirstOrDefault(x => string.Equals(x.ServiceName, Request.ServiceName, StringComparison.OrdinalIgnoreCase));
            if (typeDefinition == null)
            {
                throw new Exception(string.Format("service '{0}' cannot be found.", Request.ServiceName));
            }

            RestServiceInstanceMethodInfo methodInfo = null;
            if (Request.MethodName.HasValue())
            {
                if (Request.Request.HttpMethod == "GET")
                {
                    methodInfo = typeDefinition.InstanceMethods.OfType<RestServiceInstanceMethodInfo>()
                        .FirstOrDefault(x => x.HttpVerb == HttpVerb.Get 
                            && string.Equals(x.ServiceMethodName, Request.MethodName, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    methodInfo = typeDefinition.InstanceMethods.OfType<RestServiceInstanceMethodInfo>()
                        .FirstOrDefault(x => x.HttpVerb == HttpVerb.Post
                            && string.Equals(x.ServiceMethodName, Request.MethodName, StringComparison.OrdinalIgnoreCase));
                }
            }
            else
            {
                if (Request.Request.HttpMethod == "GET")
                {
                    methodInfo = typeDefinition.InstanceMethods.OfType<RestServiceInstanceMethodInfo>()
                        .FirstOrDefault(x => x.HttpVerb == HttpVerb.Get && x.IsDefaultMethod);
                }
                else
                {
                    methodInfo = typeDefinition.InstanceMethods.OfType<RestServiceInstanceMethodInfo>()
                        .FirstOrDefault(x => x.HttpVerb == HttpVerb.Post && x.IsDefaultMethod);
                }
            }
            if (methodInfo == null)
            {
                throw new Exception(string.Format("method '{0}' cannot be found.", Request.MethodName));
            }

            var obj = DependencyInjector.GetObject(typeDefinition.Info as Type);
            if (obj == null)
            {
                throw new Exception(string.Format("failed to create object '{0}'.", (typeDefinition.Info as Type).FullName));
            }

            object returnValue = null;
            if (Request.Request.HttpMethod == "GET")
            {
                if (methodInfo.ParameterInfos == null || methodInfo.ParameterInfos.Length == 0)
                {
                    returnValue = methodInfo.Invoke(obj, null);
                }
                else
                {
                    var dict = Request.ReadQueryString();

                    var values = new string[methodInfo.ParameterInfos.Length];

                    for (var i = 0; i < values.Length; i++)
                    {
                        var parameterInfo = methodInfo.ParameterInfos.OfType<RestServiceParameterInfo>().FirstOrDefault(x => x.Index == i);

                        var name = parameterInfo.Alias.HasValue() ? parameterInfo.Alias : parameterInfo.ParameterName;

                        if (!dict.Keys.ToArray().Exists(x => string.Equals(x, name, StringComparison.OrdinalIgnoreCase)))
                        {
                            throw new Exception(string.Format("parameter '{0}' does not exist.", name));
                        }

                        values[i] = dict.FirstOrDefault(x => string.Equals(x.Key, name, StringComparison.OrdinalIgnoreCase)).Value;
                    }

                    returnValue = methodInfo.Invoke(obj, values);
                }
            }
            else
            {
                if (methodInfo.ParameterInfos == null)
                {
                    throw new Exception(string.Format("method '{0}' must have more than one parameter.", methodInfo.MethodName));
                }
                else if (methodInfo.ParameterInfos.Length == 1)
                {
                    returnValue = methodInfo.Invoke(obj, Request.ReadInputStream(methodInfo.ParameterInfos[0].TypeDefinition.Info as Type, methodInfo.RequestDataFormat));
                }
                else // > 1
                {
                    var dict = Request.ReadQueryString();

                    var values = new object[methodInfo.ParameterInfos.Length];

                    for (var i = 0; i < values.Length; i++)
                    {
                        var parameterInfo = methodInfo.ParameterInfos.OfType<RestServiceParameterInfo>().FirstOrDefault(x => x.Index == i);

                        if (parameterInfo.Source == ParameterSource.Body)
                        {
                            values[i] = Request.ReadInputStream(parameterInfo.TypeDefinition.Info as Type, methodInfo.RequestDataFormat);
                        }
                        else if (parameterInfo.Source == ParameterSource.QueryString)
                        {
                            var name = parameterInfo.Alias.HasValue() ? parameterInfo.Alias : parameterInfo.ParameterName;

                            if (!dict.Keys.ToArray().Exists(x => string.Equals(x, name, StringComparison.OrdinalIgnoreCase)))
                            {
                                throw new Exception(string.Format("parameter '{0}' does not exist.", name));
                            }

                            values[i] = dict.FirstOrDefault(x => string.Equals(x.Key, name, StringComparison.OrdinalIgnoreCase)).Value;
                        }
                        else
                        {
                            throw new Exception(string.Format("method '{0}' parameters must specify attribute.", methodInfo.MethodName));
                        }
                    }

                    returnValue = methodInfo.Invoke(obj, values);
                }
            }

            if (returnValue != null)
            {
                Response.Write(returnValue, methodInfo.ResponseDataFormat);
            }
        }
    }
}