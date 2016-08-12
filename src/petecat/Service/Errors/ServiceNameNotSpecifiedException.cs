using System;

namespace Petecat.Service.Errors
{
    public class ServiceNameNotSpecifiedException : Exception
    {
        public ServiceNameNotSpecifiedException()
            : base("Welcome to Service Host. Please specify service name and method name to access specified service.")
        {
        }
    }
}
