using System.Web;
using System.Collections.Generic;

using Petecat.Extension;

namespace Petecat.Service
{
    public class HttpHandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            return GetHttpHandler(context.Request.RawUrl);
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }

        public bool IsReusable { get { return true; } }

        public IHttpHandler GetHttpHandler(string url)
        {
            url = url.Trim('/');

            // remove virtual path
            var virtualPath = HttpApplicationConfigManager.Instance.GetHttpRouting("VirtualPath");
            if (virtualPath.HasValue())
            {
                if (url.StartsWith(virtualPath.Trim('/')))
                {
                    url = url.Remove(0, virtualPath.Trim('/').Length);
                }
                else
                {
                    // throw exception
                }
            }

            url = url.TrimStart('/');

            // remove query string
            if (url.Contains("?"))
            {
                url = url.Remove(url.IndexOf('?'));
            }

            url = url.TrimEnd('/');

            // default routing
            if (!url.HasValue())
            {
                var defaultRouting = HttpApplicationConfigManager.Instance.GetHttpRouting("DefaultRouting");
                if (defaultRouting.HasValue())
                {
                    url = url + '/' + defaultRouting.Trim('/');
                }
            }

            // last field in url
            var lastField = url;
            if (url.HasValue() && url.Contains("/"))
            {
                lastField = url.Substring(url.LastIndexOf('/') + 1);
            }

            // static resource
            if (lastField.HasValue() && lastField.Contains("."))
            {
                return new StaticResourceHttpHandler(url);
            }
            else
            {
                return new ServiceHttpHandler(url);
            }
        }
    }
}
