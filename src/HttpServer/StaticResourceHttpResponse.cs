using System.IO;
using System.Web;

namespace Petecat.HttpServer
{
    public class StaticResourceHttpResponse : HttpResponseBase
    {
        public StaticResourceHttpResponse(HttpResponse response) : base(response)
        {
        }

        public void Write(string resourcePath)
        {
            Response.ContentType = ContentType ?? "application/octet-stream";
            Response.StatusCode = StatusCode;

            using (var inputStream = new FileStream(resourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var count = 0;
                var buffer = new byte[10 * 1024];
                while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0 && Response.IsClientConnected)
                {
                    Response.OutputStream.Write(buffer, 0, count);
                }
            }
        }

        public void Error(int statusCode)
        {
            Response.StatusCode = statusCode;
        }
    }
}
