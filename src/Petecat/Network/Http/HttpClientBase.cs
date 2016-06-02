using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Petecat.Network.Http
{
    public class HttpClientBase : IHttpClient
    {
        public HttpClientBase(string requestUri)
        {
            RequestUri = requestUri;
            RequestHeaders = new Dictionary<string, string>();
            RequestQueryString = new Dictionary<string, string>();
        }

        public string RequestUri { get; private set; }

        public Dictionary<string, string> RequestHeaders { get; private set; }

        public Dictionary<string, string> RequestQueryString { get; private set; }

        public WebProxy Proxy { get; set; }

        public int Timeout { get; set; }

        public TResponse Get<TResponse>()
        {
            var request = GetHttpWebRequest(HttpVerb.GET);

            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    return BuildResponseBody<TResponse>(response);
                }
            }
            catch (Exception e)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, "failed to get response.", new Logging.Loggers.ExceptionWrapper(e));
                return default(TResponse);
            }
        }

        public byte[] Get()
        {
            var request = GetHttpWebRequest(HttpVerb.GET);

            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        return ReadStream(stream);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, "failed to get response.", new Logging.Loggers.ExceptionWrapper(e));
                return null;
            }
        }

        public string Get(Encoding encoding)
        {
            var data = Get();
            if (data != null)
            {
                return encoding.GetString(data);
            }

            return null;
        }

        public TResponse Post<TResponse>(object requestBody)
        {
            var request = GetHttpWebRequest(HttpVerb.POST);

            try
            {
                BuildRequestBody(request, requestBody);
            }
            catch (Exception e)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, "failed to write request stream.", new Logging.Loggers.ExceptionWrapper(e));
                return default(TResponse);
            }

            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    return BuildResponseBody<TResponse>(response);
                }
            }
            catch (Exception e)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, "failed to get response.", new Logging.Loggers.ExceptionWrapper(e));
                return default(TResponse);
            }
        }

        private void BuildRequestQueryString()
        {
            var stringBuilder = new StringBuilder(RequestUri);

            if (RequestQueryString.Count > 0)
            {
                stringBuilder.Append("?");
            }

            foreach (var item in RequestQueryString)
            {
                stringBuilder.AppendFormat("{0}={1}&", item.Key, HttpUtility.UrlEncode(item.Value));
            }

            RequestUri = stringBuilder.ToString().TrimEnd('&');
        }

        private void BuildRequestHeaders(HttpWebRequest request, HttpVerb httpVerb)
        {
            request.Method = httpVerb.ToString();

            foreach (var requestHeader in RequestHeaders)
            {
                request.Headers.Add(requestHeader.Key, requestHeader.Value);
            }
        }

        private void BuildRequestBody(HttpWebRequest request, object instance)
        {
            var contentType = "";
            if (request.Headers.AllKeys.ToList().Exists(x => x.Equals("content-type", StringComparison.OrdinalIgnoreCase)))
            {
                contentType = request.Headers["content-type"].ToLower();
            }

            if (contentType == "application/xml")
            {
                using (var requestStream = request.GetRequestStream())
                {
                    Data.DataContractXml.Serializer.WriteObject(instance, requestStream);
                }
            }
            else
            {
                using (var requestStream = request.GetRequestStream())
                {
                    Data.DataContractJson.Serializer.WriteObject(instance, requestStream);
                }
            }
        }

        private TResponse BuildResponseBody<TResponse>(HttpWebResponse response)
        {
            var contentType = "";
            if (response.Headers.AllKeys.ToList().Exists(x => x.Equals("content-type", StringComparison.OrdinalIgnoreCase)))
            {
                contentType = response.Headers["content-type"].ToLower();
            }

            if (contentType == "application/xml")
            {
                using (var responseStream = response.GetResponseStream())
                {
                    return Data.DataContractXml.Serializer.ReadObject<TResponse>(responseStream);
                }
            }
            else
            {
                using (var requestStream = response.GetResponseStream())
                {
                    return Data.DataContractJson.Serializer.ReadObject<TResponse>(requestStream);
                }
            }
        }

        private byte[] ReadStream(Stream stream)
        {
            var data = new byte[0];
            var count = 0;
            var buffer = new byte[1024 * 4];
            while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var b = new byte[count];
                Array.Copy(buffer, b, count);
                data = data.Concat(b).ToArray();
            }
            return data;
        }

        private HttpWebRequest GetHttpWebRequest(HttpVerb httpVerb)
        {
            BuildRequestQueryString();

            var request = WebRequest.Create(RequestUri) as HttpWebRequest;
            request.Proxy = Proxy;
            request.Credentials = Proxy != null ? Proxy.Credentials : null;
            request.Timeout = Timeout > 0 ? Timeout : 100000;

            BuildRequestHeaders(request, httpVerb);

            return request;
        }
    }
}