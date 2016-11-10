using Petecat.Formatter;
using System.Web;

namespace Petecat.HttpServer
{
    public class RestServiceHttpResponse : HttpResponseBase
    {
        public RestServiceHttpResponse(HttpResponse response) : base(response)
        {
        }

        public void Write(object obj)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCode;

            new JsonFormatter().WriteObject(obj, Response.OutputStream);
        }

        public void Error()
        {
            Response.StatusCode = 400;
        }
    }
}
