using System;
using System.IO;
using System.Net;
using System.Text;

using Petecat.Extension;

namespace Petecat.Network.Ftp
{
    public class FtpClientResponse : IDisposable
    {
        public FtpClientResponse(FtpWebResponse response)
        {
            Response = response;

            StatusCode = response.StatusCode;
        }

        public FtpWebResponse Response { get; private set; }

        public FtpStatusCode StatusCode { get; private set; }

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
                    data = data.Append(new byte[count]);
                    Buffer.BlockCopy(buffer, 0, data, data.Length - count, count);

                    progress(data.Length, false);
                }
            }

            progress(data.Length, true);

            return data;
        }

        public string GetString(Encoding encoding)
        {
            return encoding.GetString(GetBytes());
        }

        public void Dispose()
        {
            Response.Close();
        }
    }
}
