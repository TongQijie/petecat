using System.Web;

namespace Petecat.Service
{
    public class StaticResourceHttpRequest
    {
        public StaticResourceHttpRequest(HttpRequest httpRequest)
        {
            Request = httpRequest;

            string relativePath, resourceType;
            if (!StaticResourceHttpPathHelper.TryParseStaticResourcePath(Request.RawUrl, out relativePath, out resourceType))
            {
                throw new Errors.ServiceHttpRequestInvalidVirtualPathException(Request.Url.AbsoluteUri);
            }

            RelativePath = relativePath;
            ResourceType = resourceType;
        }

        public HttpRequest Request { get; private set; }

        public string RelativePath { get; private set; }

        public string ResourceType { get; private set; }
    }
}
