using System;

namespace Petecat.Aop
{
    public static class AopProxyTypeFactory
    {
        public static object GetProxyObject(Type baseClass, IAopInterceptor aopInterceptor)
        {
            return DefaultAopProxyTypeGenerator.Instance.GetProxyObject(baseClass, aopInterceptor);
        }

        public static T GetProxyObject<T>(IAopInterceptor aopInterceptor)
        {
            return DefaultAopProxyTypeGenerator.Instance.GetProxyObject<T>(aopInterceptor);
        }
    }
}
