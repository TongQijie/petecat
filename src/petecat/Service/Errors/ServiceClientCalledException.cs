using System;

namespace Petecat.Service.Errors
{
    public class ServiceClientCallingFailedException : Exception
    {
        public ServiceClientCallingFailedException(string resourceName, string statusCode, string body)
            : base(string.Format("resource service '{0}' was called with error code '{1}'.{2}Request: {3}", resourceName, statusCode, Environment.NewLine, body))
        {
        }
    }
}
