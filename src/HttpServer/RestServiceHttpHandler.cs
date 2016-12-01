using Petecat.Logging;
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
        public RestServiceHttpHandler(RestServiceHttpRequest request, RestServiceHttpResponse response)
        {
            Request = request;
            Response = response;
        }

        public bool IsReusable { get { return true; } }

        [ThreadStatic]
        public static RestServiceHttpRequest Request;

        [ThreadStatic]
        public static RestServiceHttpResponse Response;

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var container = DependencyInjector.GetContainer<HttpServerAssemblyContainer>();
                if (container == null)
                {
                    throw new Exception("http server container is null.");
                }

                var obj = Execute(container);
                if (obj != null)
                {
                    Response.Write(obj);
                }

                Response.StatusCode = 200;
            }
            catch (Exception e)
            {
                DependencyInjector.GetObject<IFileLogger>().LogEvent("RestServiceHttpHandler", Severity.Error, "failed to process restservice request.", e);
                Response.Error();
            }
        }

        public object Execute(HttpServerAssemblyContainer container)
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
                methodInfo = typeDefinition.InstanceMethods.OfType<RestServiceInstanceMethodInfo>()
                    .FirstOrDefault(x => string.Equals(x.ServiceMethodName, Request.MethodName, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                methodInfo = typeDefinition.InstanceMethods.OfType<RestServiceInstanceMethodInfo>()
                    .FirstOrDefault(x => x.IsDefaultMethod);
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

            if (Request.Request.HttpMethod == "GET")
            {
                if (methodInfo.ParameterInfos == null || methodInfo.ParameterInfos.Length == 0)
                {
                    return methodInfo.Invoke(obj, null);
                }
                else
                {
                    var dict = Request.ReadQueryString();

                    var values = new string[methodInfo.ParameterInfos.Length];

                    for (var i = 0; i < values.Length; i++)
                    {
                        var parameterInfo = methodInfo.ParameterInfos.FirstOrDefault(x => x.Index == i);

                        if (!dict.Keys.ToArray().Exists(x => string.Equals(x, parameterInfo.ParameterName, StringComparison.OrdinalIgnoreCase)))
                        {
                            throw new Exception(string.Format("parameter '{0}' does not exist.", parameterInfo.ParameterName));
                        }

                        values[i] = dict.FirstOrDefault(x => string.Equals(x.Key, parameterInfo.ParameterName, StringComparison.OrdinalIgnoreCase)).Value;
                    }

                    return methodInfo.Invoke(obj, values);
                }
            }
            else
            {
                if (methodInfo.ParameterInfos == null || methodInfo.ParameterInfos.Length != 1)
                {
                    throw new Exception(string.Format("method '{0}' must have one parameter.", methodInfo.MethodName));
                }

                return methodInfo.Invoke(obj, Request.ReadInputStream(methodInfo.ParameterInfos[0].TypeDefinition.Info as Type, methodInfo.DataFormat));
            }
        }
    }
}