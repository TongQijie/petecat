using System.Web;
using System.Linq;

using Petecat.Data.Formatters;

namespace Petecat.Service
{
    public class ServiceHttpResponse
    {
        public ServiceHttpResponse(HttpResponse httpResponse, string[] acceptTypes)
        {
            Response = httpResponse;
            AcceptTypes = acceptTypes ?? new string[0];
        }

        public HttpResponse Response { get; private set; }

        public string[] AcceptTypes { get; private set; }

        public void SetStatusCode(int statusCode)
        {
            Response.StatusCode = statusCode;
        }

        public void WriteObject(object instance)
        {
            if (AcceptTypes.Contains("application/xml"))
            {
                Response.Write(new XmlFormatter().WriteString(instance));
            }
            else if (AcceptTypes.Contains("application/json"))
            {
                Response.Write(new DataContractJsonFormatter().WriteString(instance));
            }
            else
            {
                Response.Write(instance);
            }
        }
    }
}
