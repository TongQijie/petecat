using Petecat.DependencyInjection;
using Petecat.HttpServer.DependencyInjection;
using Petecat.Logging;
using System;
using System.Collections.Generic;
using System.Web;

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
                var obj = DependencyInjector.GetContainer<RestServiceAssemblyContainer>().Execute(Request);
                Response.StatusCode = 200;
                Response.Write(obj);
            }
            catch (Exception e)
            {
                DependencyInjector.GetObject<IFileLogger>().LogEvent("RestServiceHttpHandler", Severity.Error, "failed to process restservice request.", e);
                Response.Error();
            }
        }
    }
}
