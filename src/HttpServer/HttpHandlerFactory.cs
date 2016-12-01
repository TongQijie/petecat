using System;
using System.Web;

using Petecat.Logging;
using Petecat.Extending;
using Petecat.DependencyInjection;

namespace Petecat.HttpServer
{
    public class HttpHandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            var rawUrl = context.Request.RawUrl.Trim('/');

            // remove virtual path
            var virtualPath = DependencyInjector.GetObject<IHttpApplicationConfigurer>().GetHttpApplicationRouting("VirtualPath");
            if (virtualPath.HasValue())
            {
                if (rawUrl.StartsWith(virtualPath.Trim('/'), StringComparison.OrdinalIgnoreCase))
                {
                    rawUrl = rawUrl.Remove(0, virtualPath.Trim('/').Length);
                }
                else
                {
                    DependencyInjector.GetObject<IFileLogger>().LogEvent("HttpHandlerFactory", Severity.Error, 
                        string.Format("url '{0}' is not valid.", context.Request.RawUrl));
                }
            }

            rawUrl = rawUrl.TrimStart('/');

            // remove query string
            if (rawUrl.Contains("?"))
            {
                rawUrl = rawUrl.Remove(rawUrl.IndexOf('?'));
            }

            // rewrite rule
            rawUrl = DependencyInjector.GetObject<IHttpApplicationConfigurer>().ApplyRewriteRule(rawUrl);

            rawUrl = rawUrl.TrimEnd('/');

            // default routing
            if (!rawUrl.HasValue())
            {
                var defaultRouting = DependencyInjector.GetObject<IHttpApplicationConfigurer>().GetHttpApplicationRouting("Default");
                if (defaultRouting.HasValue())
                {
                    rawUrl = rawUrl + '/' + defaultRouting.Trim('/');
                }
            }

            // last field in url
            var lastField = rawUrl;
            if (rawUrl.HasValue() && rawUrl.Contains("/"))
            {
                lastField = rawUrl.Substring(rawUrl.LastIndexOf('/') + 1);
            }

            if (context.IsWebSocketRequest)
            {
                var fields = rawUrl.SplitByChar('/');
                return new WebSocketHandler(fields.Length > 0 ? fields[0] : null);
            }
            else if (lastField.HasValue() && lastField.Contains("."))
            {
                return new StaticResourceHttpHandler(
                    new StaticResourceHttpRequest(context.Request, "./" + rawUrl, lastField.Substring(lastField.LastIndexOf('.') + 1)),
                    new StaticResourceHttpResponse(context.Response));
            }
            else
            {
                var fields = rawUrl.SplitByChar('/');
                return new RestServiceHttpHandler(
                    new RestServiceHttpRequest(context.Request, fields.Length > 0 ? fields[0] : null, fields.Length > 1 ? fields[1] : null),
                    new RestServiceHttpResponse(context.Response));
            }
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }

        public bool IsReusable { get { return true; } }
    }
}
