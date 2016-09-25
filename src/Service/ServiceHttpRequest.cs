using System.IO;
using System.Web;
using System;
using System.Collections.Generic;
using Petecat.Extension;

namespace Petecat.Service
{
    public class ServiceHttpRequest
    {
        public ServiceHttpRequest(HttpRequest httpRequest)
        {
            Request = httpRequest;

            var fields = ServiceHttpPathHelper.Get(Request.RawUrl);
            VirtualPath = Utility.AppConfigUtility.GetAppConfig<string>("VirtualPath", null);
            if (!VirtualPath.HasValue())
            {
                if (fields.Length > 0)
                {
                    ServiceName = fields[0];
                }
                if (fields.Length > 1)
                {
                    MethodName = fields[1];
                }
            }
            else
            {
                var paths = VirtualPath.SplitByChar('/');
                for (int i = 0; i < paths.Length; i++)
                {
                    if (fields.Length <= i || !string.Equals(paths[i], fields[i], StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Errors.ServiceHttpRequestInvalidVirtualPathException(Request.Url.AbsoluteUri);
                    }
                }

                if (fields.Length > paths.Length)
                {
                    ServiceName = fields[paths.Length];
                }
                if (fields.Length > (paths.Length + 1))
                {
                    MethodName = fields[paths.Length + 1];
                }
            }
        }

        public HttpRequest Request { get; private set; }

        public string VirtualPath { get; private set; }

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
