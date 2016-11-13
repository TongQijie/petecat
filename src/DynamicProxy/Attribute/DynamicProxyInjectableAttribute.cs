using Petecat.DependencyInjection.Attribute;

namespace Petecat.DynamicProxy.Attribute
{
    public class DynamicProxyInjectableAttribute : DependencyInjectableAttribute
    {
        public DynamicProxyInjectableAttribute()
        {
            OverridedInference = true;
        }
    }
}
