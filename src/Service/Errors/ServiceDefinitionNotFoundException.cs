using System;

namespace Petecat.Service.Errors
{
    public class ServiceDefinitionNotFoundException : Exception
    {
        public ServiceDefinitionNotFoundException(string name)
            : base(string.Format("service definition {0} not found.", name))
        {
        }
    }
}
