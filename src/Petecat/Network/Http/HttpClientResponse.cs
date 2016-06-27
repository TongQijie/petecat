using System;
using System.IO;
using System.Net;
using System.Text;

using Petecat.Data.Errors;
using Petecat.Data.Formatters;

namespace Petecat.Network.Http
{
    public class HttpClientResponse : IDisposable
    {
        public HttpClientResponse(HttpWebResponse response)
        {
            Response = response;
        }

        public HttpWebResponse Response { get; private set; }

        public byte[] GetBytes()
        {
            using (var inputStream = Response.GetResponseStream())
            {
                using (var outputStream = new MemoryStream())
                {
                    inputStream.CopyTo(outputStream);
                    return outputStream.ToArray();
                }
            }
        }

        public void GetStream(Stream outputStream)
        {
            using (var inputStream = Response.GetResponseStream())
            {
                inputStream.CopyTo(outputStream);
            }
        }

        public string GetString(Encoding encoding)
        {
            return encoding.GetString(GetBytes());
        }

        public TResponse GetObject<TResponse>()
        {
            IDataFormatter dataFormatter = null;

            foreach (var contentTypeString in HttpConstants.HttpContentTypeStringMapping)
            {
                if (Response.ContentType.Contains(contentTypeString.Value))
                {
                    dataFormatter = DataFormatterUtility.Get(HttpConstants.HttpContentTypeFormatterMapping[contentTypeString.Key]);
                    break;
                }
            }

            if (dataFormatter == null)
            {
                throw new FormatterNotFoundException();
            }

            using (var responseStream = Response.GetResponseStream())
            {
                return dataFormatter.ReadObject<TResponse>(responseStream);
            }
        }

        public TResponse GetObject<TResponse>(HttpContentType contentType)
        {
            IDataFormatter dataFormatter = DataFormatterUtility.Get(HttpConstants.HttpContentTypeFormatterMapping[contentType]);

            if (dataFormatter == null)
            {
                throw new FormatterNotFoundException();
            }

            using (var responseStream = Response.GetResponseStream())
            {
                return dataFormatter.ReadObject<TResponse>(responseStream);
            }
        }

        public void Dispose()
        {
            Response.Close();
        }
    }
}
