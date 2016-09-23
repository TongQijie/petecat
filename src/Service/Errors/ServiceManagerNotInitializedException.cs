using System;

namespace Petecat.Service.Errors
{
    public class ServiceManagerNotInitializedException : Exception
    {
        public ServiceManagerNotInitializedException()
            : base("service manager has not been initialized.")
        {
        }
    }
}
