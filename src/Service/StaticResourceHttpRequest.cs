using System.Web;

namespace Petecat.Service
{
    public class StaticResourceHttpRequest
    {
        public StaticResourceHttpRequest(HttpRequest httpRequest, string relativePath, string resourceType)
        {
            Request = httpRequest;
            RelativePath = relativePath;
            ResourceType = resourceType;
        }

        public HttpRequest Request { get; private set; }

        public string RelativePath { get; private set; }

        public string ResourceType { get; private set; }
    }
}
