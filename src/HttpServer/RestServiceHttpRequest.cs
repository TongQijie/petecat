using Petecat.Formatter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Petecat.HttpServer
{
    public class RestServiceHttpRequest : HttpRequestBase
    {
        public RestServiceHttpRequest(HttpRequest request, string serviceName, string methodName) 
            : base(request)
        {
            ServiceName = serviceName;
            MethodName = methodName;
        }

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

        public object ReadInputStream(Type targetType)
        {
            var inputStream = Request.InputStream;
            inputStream.Seek(0, SeekOrigin.Begin);

            return new JsonFormatter().ReadObject(targetType, inputStream);
        }

        public object ReadInputStream<T>()
        {
            var inputStream = Request.InputStream;
            inputStream.Seek(0, SeekOrigin.Begin);

            return new JsonFormatter().ReadObject<T>(inputStream);
        }
    }
}
