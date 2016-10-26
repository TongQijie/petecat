using Petecat.Logging;
using Petecat.Extension;

using System;
using System.Web;

namespace Petecat.Service
{
    public class StaticResourceHttpHandler : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            var response = new StaticResourceHttpResponse(context.Response);

            try
            {
                var request = new StaticResourceHttpRequest(context.Request);
                InternalProcessRequest(request, response);
                response.SetStatusCode(200);
            }
            catch (Exception e)
            {
                LoggerManager.GetLogger().LogEvent("StaticResourceHttpHandler", LoggerLevel.Error, e);
                response.WriteString(e.Message);
                response.SetStatusCode(400);
            }
        }

        private void InternalProcessRequest(StaticResourceHttpRequest request, StaticResourceHttpResponse response)
        {
            if (string.Equals(request.ResourceType, "html", StringComparison.OrdinalIgnoreCase))
            {
                response.Write(("./" + request.RelativePath).FullPath(), "text/html");
            }
            else
            {
                response.Write(("./" + request.RelativePath).FullPath(), "application/octet-stream");
            }
        }
    }
}
