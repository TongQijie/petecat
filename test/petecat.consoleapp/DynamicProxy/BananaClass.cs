using Petecat.DynamicProxy.Attribute;
using Petecat.DynamicProxy.Interceptor;

namespace Petecat.ConsoleApp.DynamicProxy
{
    [DynamicProxyInjectable(Inference = typeof(IBananaInterface), Singleton = true)]
    public class BananaClass : IBananaInterface
    {
        [MethodInterceptor(Type = typeof(ITimingInterceptor))]
        public virtual int F(int a, int b)
        {
            Console.ConsoleBridging.WriteLine("run in F with " + (int)(a + b));
            return a + b;
        }
    }
}
