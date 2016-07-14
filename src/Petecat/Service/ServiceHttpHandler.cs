using System.Web;
using System.Linq;
using System;

namespace Petecat.Service
{
    public class ServiceHttpHandler : IHttpHandler
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            var request = new ServiceHttpRequest(context.Request);
            var response = new ServiceHttpResponse(context.Response, context.Request.AcceptTypes);

            try
            {
                InternalProcessRequest(request, response);
                response.SetStatusCode(200);
            }
            catch (Exception e)
            {
                response.SetStatusCode(400);
                response.WriteObject(e.Message);
            }
        }

        private void InternalProcessRequest(ServiceHttpRequest request, ServiceHttpResponse response)
        {
            var parts = ServiceHttpPathHelper.Get(request.Request.RawUrl).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            if (parts.Length == 0 || parts.Length > 2)
            {
                throw new Exception("Welcome to Service Host. Please specify service name and method name to access specified service.");
            }

            if (ServiceManager.Instance == null)
            {
                throw new Exception("Service Manager has not initialized.");
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
                throw new Exception(string.Format("Http Method '{0}' not support now.", request.Request.HttpMethod));
            }
        }
    }
}