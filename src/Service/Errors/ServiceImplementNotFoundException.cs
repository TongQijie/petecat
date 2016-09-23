using System;

namespace Petecat.Service.Errors
{
    public class ServiceImplementNotFoundException : Exception
    {
        public ServiceImplementNotFoundException(string name)
            : base(string.Format("service implement {0} not found.", name))
        {
        }
    }
}
