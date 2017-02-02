using System;
using System.Net;
using System.Web;
using System.Text;
using System.Collections.Generic;

using Petecat.Logging;
using Petecat.Formatter;
using Petecat.DependencyInjection;

namespace Petecat.Network.Http
{
    public class HttpRequest
    {
        private HttpRequest(HttpVerb httpVerb)
        {
            HttpVerb = httpVerb;
        }

        public HttpRequest(HttpVerb httpVerb, string uri)
            : this(httpVerb)
        {
            Request = WebRequest.Create(uri) as HttpWebRequest;
            Request.Method = HttpVerb.ToString();
        }

        public HttpRequest(HttpVerb httpVerb, string uri, Dictionary<string, string> queryStringKeyValues)
            : this(httpVerb)
        {
            var stringBuilder = new StringBuilder(uri);

            if (queryStringKeyValues != null && queryStringKeyValues.Count > 0)
            {
                if (uri.Contains("?"))
                {
                    stringBuilder.Append("&");
                }
                else
                {
                    stringBuilder.Append("?");
                }
                
                stringBuilder.Append(UrlEncodedString(queryStringKeyValues));
            }

            Request = WebRequest.Create(stringBuilder.ToString()) as HttpWebRequest;
            Request.Method = HttpVerb.ToString();
        }

        public HttpWebRequest Request { get; private set; }

        public HttpVerb HttpVerb { get; private set; }

        public object Body { get; private set; }

        public void SetRequestBody(object requestBody)
        {
            Body = requestBody;

            using (var requestStream = Request.GetRequestStream())
            {
                DependencyInjector.GetObject<IJsonFormatter>().WriteObject(requestBody, requestStream);
            }
        }

        public HttpResponse GetResponse()
        {
            try
            {
                return new HttpResponse(Request.GetResponse() as HttpWebResponse);
            }
            catch (WebException e)
            {
                if (e.Response != null && e.Response is HttpWebResponse)
                {
                    return new HttpResponse(e.Response as HttpWebResponse);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public HttpResponse GetResponse(object requestBody)
        {
            SetRequestBody(requestBody);
            return GetResponse();
        }

        private string UrlEncodedString(Dictionary<string, string> keyValues)
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in keyValues)
            {
                stringBuilder.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(item.Key), HttpUtility.UrlEncode(item.Value));
            }

            return stringBuilder.ToString().TrimEnd('&');
        }
    }
}
