using System;

namespace Petecat.Service.Errors
{
    public class ServiceResourceNotFoundException : Exception
    {
        public ServiceResourceNotFoundException(string resourceName)
            : base(string.Format("service resource '{0}' cannot be found in Configuration/ServiceResources.config.", resourceName))
        {
        }
    }
}
