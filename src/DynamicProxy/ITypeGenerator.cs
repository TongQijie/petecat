using System;
namespace Petecat.DynamicProxy
{
    public interface ITypeGenerator
    {
        object CreateProxyObject(Type baseClass, IInterceptor interceptor);

        T CreateProxyObject<T>(IInterceptor interceptor);

        Type CreateProxyType(Type baseClass);
    }
}
