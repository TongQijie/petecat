using Petecat.Formatter;
using Petecat.DependencyInjection;

using System;
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

            DependencyInjector.GetObject<IJsonFormatter>().WriteObject(obj, Response.OutputStream);
        }

        public void Error()
        {
            Response.StatusCode = 500;
        }

        public void SetCookie(string name, string value)
        {
            Response.SetCookie(new HttpCookie(name, value));
        }

        public void SetCookie(string name, string value, DateTime expires)
        {
            Response.SetCookie(new HttpCookie(name, value) { Expires = expires });
        }
    }
}