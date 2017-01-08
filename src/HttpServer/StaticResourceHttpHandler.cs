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
        public StaticResourceHttpHandler(string resourcePath, string resourceType)
        {
            ResourcePath = resourcePath;
            ResourceType = resourceType;
        }

        public bool IsReusable { get { return true; } }

        public string ResourcePath { get; private set; }

        public string ResourceType { get; private set; }

        public StaticResourceHttpRequest Request { get; private set; }

        public StaticResourceHttpResponse Response { get; private set; }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                Request = new StaticResourceHttpRequest(context.Request, ResourcePath, ResourceType);
                Response = new StaticResourceHttpResponse(context.Response);

                var contentType = DependencyInjector.GetObject<IHttpApplicationConfigurer>().GetStaticResourceMapping(Request.ResourceType);
                if (contentType.HasValue())
                {
                    Response.ContentType = contentType;
                }
                else
                {
                    Response.Error(415, "resource type does not support.");
                    return;
                }

                if (!File.Exists(Request.ResourcePath.FullPath()))
                {
                    Response.Error(404, "resource cannot be found.");
                    return;
                }

                Response.StatusCode = 200;
                Response.Write(Request.ResourcePath.FullPath());
            }
            catch (Exception e)
            {
                DependencyInjector.GetObject<IFileLogger>().LogEvent("StaticResourceHttpHandler", Severity.Error, "failed to process static resource request.", e);
                Response.Error(500, "error occurs when processing request.");
            }
        }
    }
}
