using Petecat.DependencyInjection;
using Petecat.Extension;
using System;
using System.Web;

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
                Response.Write(Request.ResourcePath);
            }
            catch (Exception e)
            {
                // TODO: log
                Response.Error();
            }
        }
    }
}
