using Petecat.Extending;
using Petecat.DependencyInjection;

using System;
using System.IO;
using System.Web;
using Petecat.Logging;

namespace Petecat.HttpServer
{
    public class StaticResourceHttpHandler : IHttpHandler
    {
        public StaticResourceHttpHandler(StaticResourceHttpRequest request, StaticResourceHttpResponse response)
        {
            Request = request;
            Response = response;
        }

        public bool IsReusable { get { return true; } }

        public StaticResourceHttpRequest Request { get; private set; }

        public StaticResourceHttpResponse Response { get; private set; }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var contentType = DependencyInjector.GetObject<IHttpApplicationConfigurer>().GetStaticResourceMapping(Request.ResourceType);
                if (contentType.HasValue())
                {
                    Response.ContentType = contentType;
                }

                if (!File.Exists(Request.ResourcePath.FullPath()))
                {
                    Response.Error(403);
                    return;
                }

                Response.StatusCode = 200;
                Response.Write(Request.ResourcePath.FullPath());
            }
            catch (Exception e)
            {
                DependencyInjector.GetObject<IFileLogger>().LogEvent("StaticResourceHttpHandler", Severity.Error, "failed to process static resource request.", e);
                Response.Error(400);
            }
        }
    }
}
