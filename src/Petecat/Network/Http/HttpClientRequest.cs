using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;

using Petecat.Data.Formatters;

namespace Petecat.Network.Http
{
    public class HttpClientRequest
    {
        private HttpClientRequest(HttpVerb httpVerb)
        {
            HttpVerb = httpVerb;
        }

        public HttpClientRequest(HttpVerb httpVerb, string uri)
            : this(httpVerb)
        {
            Request = WebRequest.Create(uri) as HttpWebRequest;
            Request.Method = HttpVerb.ToString();
        }

        public HttpClientRequest(HttpVerb httpVerb, string uri, Dictionary<string, string> queryStringKeyValues)
            : this(httpVerb)
        {
            var stringBuilder = new StringBuilder(uri);

            if (queryStringKeyValues != null || queryStringKeyValues.Count > 0)
            {
                stringBuilder.Append("?");
                stringBuilder.Append(UrlEncodedString(queryStringKeyValues));
            }

            Request = WebRequest.Create(stringBuilder.ToString()) as HttpWebRequest;
            Request.Method = HttpVerb.ToString();
        }

        public HttpWebRequest Request { get; private set; }

        private HttpContentType _RequestContentType = HttpContentType.None;

        public HttpContentType RequestContentType
        {
            get { return _RequestContentType; }
            set
            {
                if (_RequestContentType != value)
                {
                    _RequestContentType = value;
                    if (HttpConstants.HttpContentTypeStringMapping.ContainsKey(_RequestContentType))
                    {
                        Request.ContentType = HttpConstants.HttpContentTypeStringMapping[_RequestContentType];
                    }
                    else
                    {
                        Request.ContentType = "";
                    }
                }
            }
        }

        public HttpVerb HttpVerb { get; private set; }

        public void SetRequestBody(HttpContentType contentType, object requestBody)
        {
            RequestContentType = contentType;

            if (contentType == HttpContentType.FormUrlEncoded)
            {
                var keyValues = requestBody as Dictionary<string, string>;
                if (keyValues == null)
                {
                    throw new FormatException("type of request body is not dictionary.");
                }

                var data = Encoding.UTF8.GetBytes(UrlEncodedString(keyValues));

                Request.ContentLength = data.Length;

                using (var requestStream = Request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }
            }
            else
            {
                IDataFormatter dataFormatter = null;

                if (HttpConstants.HttpContentTypeFormatterMapping.ContainsKey(contentType))
                {
                    dataFormatter = DataFormatterUtility.Get(HttpConstants.HttpContentTypeFormatterMapping[contentType]);
                }

                if (dataFormatter == null)
                {
                    throw new NotSupportedException("formatter not found.");
                }

                using (var requestStream = Request.GetRequestStream())
                {
                    dataFormatter.WriteObject(requestBody, requestStream);
                }
            }
        }

        public void SetRequestBody(object requestBody)
        {
            var objectFormatter = HttpFormatterSelector.Get(Request.ContentType);
            if (objectFormatter == null)
            {
                throw new Exception(string.Format("cannot find object formatter for contenttype '{0}'", Request.ContentType));
            }

            using (var requestStream = Request.GetRequestStream())
            {
                objectFormatter.WriteObject(requestBody, requestStream);
            }
        }

        public HttpClientResponse GetResponse()
        {
            try
            {
                return new HttpClientResponse(Request.GetResponse() as HttpWebResponse);
            }
            catch (WebException e)
            {
                return new HttpClientResponse(e.Response as HttpWebResponse);
            }
        }

        public HttpClientResponse GetResponse(HttpContentType contentType, object requestBody)
        {
            SetRequestBody(contentType, requestBody);
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
