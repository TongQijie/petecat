using System.Web;

namespace Petecat.Service
{
    public class ServiceHttpHandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            return new ServiceHttpHandler();
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }

        public bool IsReusable { get { return false; } }
    }
}
