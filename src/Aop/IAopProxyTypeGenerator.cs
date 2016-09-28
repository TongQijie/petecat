using System;

namespace Petecat.Aop
{
    internal interface IAopProxyTypeGenerator
    {
        object GetProxyObject(Type baseClass, IAopInterceptor aopInterceptor);

        T GetProxyObject<T>(IAopInterceptor aopInterceptor);
    }
}
