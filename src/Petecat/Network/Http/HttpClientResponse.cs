using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

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
                return ReadStream(inputStream);
            }
        }

        public void GetStream(Stream outputStream)
        {
            using (var inputStream = Response.GetResponseStream())
            {
                ReadStream(inputStream, outputStream);
            }
        }

        public string GetString(Encoding encoding)
        {
            var data = GetBytes();
            if (data != null)
            {
                return encoding.GetString(data);
            }

            return null;
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
                throw new NotSupportedException("formatter not found.");
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
                throw new NotSupportedException("formatter not found.");
            }

            using (var responseStream = Response.GetResponseStream())
            {
                return dataFormatter.ReadObject<TResponse>(responseStream);
            }
        }

        public void Dispose()
        {
            Response.Dispose();
        }

        private byte[] ReadStream(Stream inputStream)
        {
            var data = new byte[0];
            var count = 0;
            var buffer = new byte[1024 * 4];
            while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var b = new byte[count];
                Array.Copy(buffer, b, count);
                data = data.Concat(b).ToArray();
            }
            return data;
        }

        private void ReadStream(Stream inputStream, Stream outputStream)
        {
            var count = 0;
            var buffer = new byte[1024 * 4];
            while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputStream.Write(buffer, 0, count);
            }
        }
    }
}
