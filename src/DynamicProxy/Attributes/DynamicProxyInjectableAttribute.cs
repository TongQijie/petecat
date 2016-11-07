using Petecat.DependencyInjection.Attributes;

namespace Petecat.DynamicProxy.Attributes
{
    public class DynamicProxyInjectableAttribute : DependencyInjectableAttribute
    {
        public DynamicProxyInjectableAttribute()
        {
            OverridedInference = true;
        }
    }
}
