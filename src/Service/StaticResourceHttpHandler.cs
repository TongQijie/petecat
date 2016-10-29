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
            var types = ServiceRoutingManager.Instance.GetRoutingRule("SupportedStaticResource");
            if (types.HasValue() && !types.SplitByChar(';').Exists(x => string.Equals(request.ResourceType, x, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception(string.Format("resource type '{0}' does not support.", request.ResourceType));
            }

            var contentType = ServiceHttpApplicationConfigManager.Instance.GetStaticResourceContentMapping(request.ResourceType);
            if (contentType == null)
            {
                contentType = "application/octet-stream";
            }

            response.Write(("./" + request.RelativePath).FullPath(), contentType);
        }
    }
}
