using System.Web;

namespace Petecat.HttpServer
{
    public class HttpResponseBase
    {
        public HttpResponseBase(HttpResponse response)
        {
            Response = response;
        }

        public HttpResponse Response { get; private set; }

        public string ContentType { get; set; }

        public int StatusCode { get; set; }
    }
}
