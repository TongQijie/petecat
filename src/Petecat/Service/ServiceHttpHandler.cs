using System.Web;

namespace Petecat.Service
{
    public class ServiceHttpHandler : IHttpHandler
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write("hello, World!!!!");
        }
    }
}
