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

        public void Write(object obj, DataFormat dataFormat)
        {
            if (dataFormat == DataFormat.Json)
            {
                Response.ContentType = "application/json";
                DependencyInjector.GetObject<IJsonFormatter>().WriteObject(obj, Response.OutputStream);
            }
            else if (dataFormat == DataFormat.Xml)
            {
                Response.ContentType = "application/xml";
                DependencyInjector.GetObject<IXmlFormatter>().WriteObject(obj, Response.OutputStream);
            }
            else if (dataFormat == DataFormat.Text)
            {
                Response.ContentType = "text/plain";
                Response.Write(obj.ToString());
            }
            else
            {
                Response.ContentType = "application/json";
                DependencyInjector.GetObject<IJsonFormatter>().WriteObject(obj, Response.OutputStream);
            }
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