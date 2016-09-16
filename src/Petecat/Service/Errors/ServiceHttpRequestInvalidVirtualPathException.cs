using System;

namespace Petecat.Service.Errors
{
    public class ServiceHttpRequestInvalidVirtualPathException : Exception
    {
        public ServiceHttpRequestInvalidVirtualPathException(string url)
            : base(string.Format("request url '{0}' contains invalid virtual path.", url))
        {
        }
    }
}
