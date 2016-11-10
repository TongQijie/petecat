using System.Web;

namespace Petecat.HttpServer
{
    public class HttpRequestBase
    {
        public HttpRequestBase(HttpRequest request)
        {
            Request = request;
        }

        public HttpRequest Request { get; private set; }
    }
}
