using System.Web;

namespace Petecat.HttpServer
{
    public class StaticResourceHttpRequest : HttpRequestBase
    {
        public StaticResourceHttpRequest(HttpRequest request, string resourcePath, string resourceType)
            : base(request)
        {
            ResourcePath = resourcePath;
            ResourceType = resourceType;
        }

        public string ResourcePath { get; private set; }

        public string ResourceType { get; private set; }
    }
}
