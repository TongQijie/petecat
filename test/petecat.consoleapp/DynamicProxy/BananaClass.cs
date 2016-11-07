using Petecat.DynamicProxy.Attributes;

namespace Petecat.ConsoleApp.DynamicProxy
{
    [DynamicProxyInjectable(Inference = typeof(IBananaInterface), Sington = true)]
    public class BananaClass : IBananaInterface
    {
        [MethodInterceptor(Type = typeof(IAppleInterface))]
        public virtual int F(int a, int b)
        {
            Console.ConsoleBridging.WriteLine("run in F with " + (int)(a + b));
            return a + b;
        }
    }
}
