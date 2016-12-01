using Petecat.Extending;
using Petecat.Formatter;
using Petecat.DependencyInjection;

using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Text;

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

        public object ReadInputStream(Type targetType, RestServiceDataFormat dataFormat)
        {
            var inputStream = Request.InputStream;
            inputStream.Seek(0, SeekOrigin.Begin);

            if (dataFormat == RestServiceDataFormat.Json)
            {
                return DependencyInjector.GetObject<IJsonFormatter>().ReadObject(targetType, inputStream);
            }
            else if (dataFormat == RestServiceDataFormat.Xml)
            {
                return DependencyInjector.GetObject<IXmlFormatter>().ReadObject(targetType, inputStream);
            }
            else if (dataFormat == RestServiceDataFormat.Text)
            {
                var data = new byte[0];
                var count = 0;
                var buffer = new byte[1024];
                while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    data = data.Append(buffer, 0, count);
                }

                return Encoding.UTF8.GetString(data);
            }
            else
            {
                if (Request.ContentType.Contains("application/xml") || Request.ContentType.Contains("text/xml"))
                {
                    return DependencyInjector.GetObject<IXmlFormatter>().ReadObject(targetType, inputStream);
                }
                else if (Request.ContentType.Contains("application/json"))
                {
                    return DependencyInjector.GetObject<IJsonFormatter>().ReadObject(targetType, inputStream); 
                }
                else
                {
                    var data = new byte[0];
                    var count = 0;
                    var buffer = new byte[1024];
                    while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        data = data.Append(buffer, 0, count);
                    }

                    return Encoding.UTF8.GetString(data);
                }
            }
        }

        public T ReadInputStream<T>(RestServiceDataFormat dataFormat)
        {
            return (T)ReadInputStream(typeof(T), dataFormat);
        }

        private Dictionary<string, string> _Headers = null;

        public Dictionary<string, string> Headers
        {
            get
            {
                if (_Headers == null)
                {
                    _Headers = new Dictionary<string, string>();
                    foreach (var key in Request.Headers.AllKeys)
                    {
                        _Headers.Add(key, Request.Headers[key]);
                    }
                }

                return _Headers;
            }
        }

        private Dictionary<string, string> _Cookies = null;

        public Dictionary<string, string> Cookies
        {
            get
            {
                if (_Cookies == null)
                {
                    _Cookies = new Dictionary<string, string>();
                    foreach (var key in Request.Cookies.AllKeys)
                    {
                        _Cookies.Add(key, Request.Cookies[key].Value);
                    }
                }

                return _Cookies;
            }
        }
    }
}