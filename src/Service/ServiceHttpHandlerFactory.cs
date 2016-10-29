using System.Web;

using Petecat.Extension;

namespace Petecat.Service
{
    public class ServiceHttpHandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            if (!context.Request.RawUrl.HasValue())
            {
                var defaultPath = ServiceHttpApplicationConfigManager.Instance.GetServiceHttpRouting("DefaultPath");
                if (defaultPath != null)
                {
                    var path = context.Request.RawUrl.TrimEnd('/') + "/" + defaultPath;
                }
            }

            if (IsStaticResourceHandler(context.Request.RawUrl))
            {
                return new StaticResourceHttpHandler();
            }
            else
            {
                return new ServiceHttpHandler();
            }
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }

        public bool IsReusable { get { return true; } }

        public bool IsStaticResourceHandler(string url)
        {
            url = url.TrimStart('/');
            if (url.Contains("?"))
            {
                url = url.Remove(url.IndexOf('?'));
            }

            return url.Contains(".");
        }
    }
}
