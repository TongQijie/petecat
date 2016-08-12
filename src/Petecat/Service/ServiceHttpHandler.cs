using System.Web;
using System.Linq;
using System;

using Petecat.Logging;

namespace Petecat.Service
{
    public class ServiceHttpHandler : IHttpHandler
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            var request = new ServiceHttpRequest(context.Request);
            var response = new ServiceHttpResponse(context.Response, context.Request.AcceptTypes);

            response.SetStatusCode(200);
            try
            {
                InternalProcessRequest(request, response);
            }
            catch (Exception e)
            {
                LoggerManager.GetLogger().LogEvent("ServiceHttpHandler", LoggerLevel.Error, e);
                response.WriteString(e.Message);
            }
        }

        private void InternalProcessRequest(ServiceHttpRequest request, ServiceHttpResponse response)
        {
            var parts = ServiceHttpPathHelper.Get(request.Request.RawUrl);
            if (string.IsNullOrEmpty(request.ServiceName))
            {
                throw new Errors.ServiceNameNotSpecifiedException();
            }

            if (ServiceManager.Instance == null)
            {
                throw new Errors.ServiceManagerNotInitializedException();
            }

            if (request.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                ServiceManager.Instance.InvokeGet(request, response);
            }
            else if (request.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                ServiceManager.Instance.InvokePost(request, response);
            }
            else
            {
                throw new Errors.ServiceHttpMethodNotSupportException(request.Request.HttpMethod);
            }
        }
    }
}