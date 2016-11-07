using System;

namespace Petecat.DynamicProxy.Errors
{
    public class InterceptorNotFoundException : Exception
    {
        public InterceptorNotFoundException(Type type)
            : base(string.Format("interceptor '{0}' cannot be found.", type.FullName))
        {
        }
    }
}
