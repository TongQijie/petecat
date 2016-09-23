using System;

namespace Petecat.Service.Errors
{
    public class ServiceMethodNotMatchedException : Exception
    {
        public ServiceMethodNotMatchedException(string methodName)
            : base(string.Format("service method {0} not matched or missing.", methodName))
        {
        }
    }
}
