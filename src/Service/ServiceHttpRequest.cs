using System;
using System.IO;
using System.Web;
using System.Collections.Generic;

namespace Petecat.Service
{
    public class ServiceHttpRequest
    {
        public ServiceHttpRequest(HttpRequest httpRequest)
        {
            Request = httpRequest;

            string serviceName, methodName;
            if (!ServiceHttpPathHelper.TryParseServiceUri(Request.RawUrl, out serviceName, out methodName))
            {
                throw new Errors.ServiceHttpRequestInvalidVirtualPathException(Request.Url.AbsoluteUri);
            }

            ServiceName = serviceName;
            MethodName = methodName;
        }

        public HttpRequest Request { get; private set; }

        public string ServiceName { get; private set; }

        public string MethodName { get; private set; }
        
        public Dictionary<string, string> ReadQueryString()
        {
            var parameters = new Dictionary<string, string>();

            foreach (var key in Request.QueryString.AllKeys)
            {
                parameters.Add(key, Request.QueryString[key]);
            }

            return parameters;
        }

        public object ReadObject(Type targetType)
        {
            var inputStream = Request.InputStream;
            inputStream.Seek(0, SeekOrigin.Begin);

            var objectFormatter = ServiceHttpFormatter.GetFormatter(Request.ContentType);
            if (objectFormatter != null)
            {
                return objectFormatter.ReadObject(targetType, inputStream);
            }
            else
            {
                using (var memoryStream = new MemoryStream())
                {
                    inputStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
