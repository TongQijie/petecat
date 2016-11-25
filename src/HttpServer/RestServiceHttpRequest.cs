using Petecat.Formatter;
using Petecat.DependencyInjection;

using System;
using System.IO;
using System.Web;
using System.Collections.Generic;

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

            return DependencyInjector.GetObject<IJsonFormatter>().ReadObject(targetType, inputStream);
        }

        public object ReadInputStream<T>()
        {
            var inputStream = Request.InputStream;
            inputStream.Seek(0, SeekOrigin.Begin);

            return DependencyInjector.GetObject<IJsonFormatter>().ReadObject<T>(inputStream);
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