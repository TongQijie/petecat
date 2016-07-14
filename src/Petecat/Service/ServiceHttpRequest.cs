using System.IO;
using System.Web;
using System;
using System.Collections.Generic;

using Petecat.Data.Formatters;

namespace Petecat.Service
{
    public class ServiceHttpRequest
    {
        public ServiceHttpRequest(HttpRequest httpRequest)
        {
            Request = httpRequest;

            var fields = ServiceHttpPathHelper.Get(Request.RawUrl);
            if (fields.Length > 0)
            {
                ServiceName = fields[0];
            }
            if (fields.Length > 1)
            {
                MethodName = fields[1];
            }
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

            if (Request.ContentType.Contains("application/xml"))
            {
                return new XmlFormatter().ReadObject(targetType, inputStream);
            }
            else if (Request.ContentType.Contains("application/json"))
            {
                return new DataContractJsonFormatter().ReadObject(targetType, inputStream);
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
