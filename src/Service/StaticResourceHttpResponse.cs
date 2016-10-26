using System.IO;
using System.Web;

namespace Petecat.Service
{
    public class StaticResourceHttpResponse
    {
        public StaticResourceHttpResponse(HttpResponse response)
        {
            Response = response;
        }

        public HttpResponse Response { get; private set; }

        

        public void Write(string path, string contentType)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            Response.ContentType = contentType;

            using (var inputStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var count = 0;
                var buffer = new byte[10 * 1024];
                while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0 && Response.IsClientConnected)
                {
                    Response.OutputStream.Write(buffer, 0, count);
                }
            }
        }

        public void SetStatusCode(int statusCode)
        {
            Response.StatusCode = statusCode;
        }

        public void WriteString(string text)
        {
            Response.Write(text);
        }
    }
}
