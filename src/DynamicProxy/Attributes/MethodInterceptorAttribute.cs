using System;

namespace Petecat.DynamicProxy.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodInterceptorAttribute : Attribute
    {
        public Type Type { get; set; }
    }
}
