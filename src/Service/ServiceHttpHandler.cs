using System;
using System.Web;

using Petecat.Logging;
using Petecat.Extending;
using Petecat.DependencyInjection;

namespace Petecat.Service
{
    public class ServiceHttpHandler : HttpHandlerBase, IHttpHandler
    {
        public ServiceHttpHandler(string handledUrl)
            : base(handledUrl)
        {
        }

        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            var response = new ServiceHttpResponse(context.Response, context.Request.AcceptTypes);

            try
            {
                var fields = HandledUrl.SplitByChar('/');
                var request = new ServiceHttpRequest(context.Request, 
                    fields.Length > 0 ? fields[0] : null, 
                    fields.Length > 1 ? fields[1] : null);
                InternalProcessRequest(request, response);
                response.SetStatusCode(200);
            }
            catch (Exception e)
            {
                DependencyInjector.GetObject<IFileLogger>().LogEvent("ServiceHttpHandler", Severity.Error, e);
                response.WriteString(e.Message);
                response.SetStatusCode(400);
            }
        }

        private void InternalProcessRequest(ServiceHttpRequest request, ServiceHttpResponse response)
        {
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