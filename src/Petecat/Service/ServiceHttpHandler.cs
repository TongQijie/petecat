using System.Web;
using System.Linq;
using System;
using System.Collections.Generic;

using Petecat.Data.Formatters;

namespace Petecat.Service
{
    public class ServiceHttpHandler : IHttpHandler
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            var parts = ServiceHttpPathHelper.Get(context.Request.RawUrl).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            if (parts.Length == 0 || parts.Length > 2)
            {
                context.Response.Write("Welcome to Service Host. Please specify service name and method name to access specified service.");
                return;
            }

            if (ServiceManager.Instance == null)
            {
                context.Response.Write("Service Manager has not initialized.");
                return;
            }

            if (context.Request.HttpMethod == "GET")
            {
                var parameters = new Dictionary<string, object>();
                foreach (var key in context.Request.QueryString.Keys)
                {
                    parameters.Add(key.ToString(), context.Request.QueryString[key.ToString()]);
                }

                try
                {
                    var response = ServiceManager.Instance.Invoke(parts[0], parts.Length > 1 ? parts[1] : "", parameters, context.Response.ContentType);
                    context.Response.Write(response);
                }
                catch (Exception e)
                {
                    context.Response.Write("Error: " + e.Message);
                }
            }
            else if (context.Request.HttpMethod == "POST")
            {
                var response = ServiceManager.Instance.Invoke(parts[0], parts.Length > 1 ? parts[1] : "", context.Request.ContentType, context.Request.InputStream, context.Response.ContentType);
                context.Response.Write(response);
            }
            else
            {
                context.Response.Write(string.Format("Http Method '{0}' not support now.", context.Request.HttpMethod));
            }
        }
    }
}
