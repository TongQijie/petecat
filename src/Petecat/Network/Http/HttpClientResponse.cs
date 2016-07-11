using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;

using Petecat.Data.Errors;
using Petecat.Data.Formatters;

namespace Petecat.Network.Http
{
    public class HttpClientResponse : IDisposable
    {
        public HttpClientResponse(HttpWebResponse response)
        {
            Response = response;

            StatusCode = response.StatusCode;
        }

        public HttpStatusCode StatusCode { get; private set; }

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

        public byte[] GetBytes(Action<int, bool> progress, int bufferSize = 4 * 1024)
        {
            var data = new byte[0];

            using (var inputStream = Response.GetResponseStream())
            {
                var buffer = new byte[bufferSize];
                int count = 0;

                while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    data = data.Concat(new byte[count]).ToArray();
                    Buffer.BlockCopy(buffer, 0, data, data.Length - count, count);

                    progress(data.Length, false);
                }
            }

            progress(data.Length, true);

            return data;
        }

        public void GetStream(Stream outputStream)
        {
            using (var inputStream = Response.GetResponseStream())
            {
                inputStream.CopyTo(outputStream);
            }
        }

        public void GetStream(Stream outputStream, Action<int, bool> progress, int bufferSize = 4 * 1024)
        {
            var totalReadCount = 0;

            using (var inputStream = Response.GetResponseStream())
            {
                var buffer = new byte[bufferSize];
                int count = 0;

                while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outputStream.Write(buffer, 0, count);
                    totalReadCount += count;

                    progress(totalReadCount, false);
                }
            }

            progress(totalReadCount, true);
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
