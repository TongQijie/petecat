using Petecat.DependencyInjection;
using Petecat.HttpServer.DependencyInjection;

using System;
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

        public RestServiceHttpRequest Request { get; private set; }

        public RestServiceHttpResponse Response { get; private set; }

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
                // TODO: log
                Response.Error();
            }
        }
    }
}
