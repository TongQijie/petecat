using Petecat.Logging;
using Petecat.Extending;
using Petecat.DependencyInjection;

using System;
using System.Web;

namespace Petecat.Service
{
    public class StaticResourceHttpHandler : HttpHandlerBase, IHttpHandler
    {
        public StaticResourceHttpHandler(string handledUrl)
            : base(handledUrl)
        {
        }

        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            var response = new StaticResourceHttpResponse(context.Response);

            try
            {
                var request = new StaticResourceHttpRequest(context.Request, 
                    HandledUrl, 
                    HandledUrl.Substring(HandledUrl.LastIndexOf('.') + 1));
                InternalProcessRequest(request, response);
                response.SetStatusCode(200);
            }
            catch (Exception e)
            {
                DependencyInjector.GetObject<IFileLogger>().LogEvent("StaticResourceHttpHandler", Severity.Error, e);
                response.WriteString(e.Message);
                response.SetStatusCode(400);
            }
        }

        private void InternalProcessRequest(StaticResourceHttpRequest request, StaticResourceHttpResponse response)
        {
            var contentType = HttpApplicationConfigManager.Instance.GetStaticResourceContentMapping(request.ResourceType);
            if (contentType == null)
            {
                throw new Errors.StaticResourceNotSupportedException(request.ResourceType);
            }
            else if (contentType == string.Empty)
            {
                contentType = "application/octet-stream";
            }

            response.Write(("./" + request.RelativePath).FullPath(), contentType);
        }
    }
}
