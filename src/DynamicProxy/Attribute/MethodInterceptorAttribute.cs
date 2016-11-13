namespace Petecat.DynamicProxy.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class MethodInterceptorAttribute : Attribute
    {
        public Type Type { get; set; }
    }
}
