using Petecat.DependencyInjection.Attributes;
using Petecat.DynamicProxy.Attributes;
namespace Petecat.ConsoleApp.DynamicProxy
{
    [DependencyInjectable(Sington = true)]
    public class BananaClass
    {
        //public BananaClass(AppleClass apple)
        //{
        //}

        [MethodInterceptor(Type = typeof(IAppleInterface))]
        public virtual int F(int a, int b)
        {
            Console.ConsoleBridging.WriteLine("run in F with " + (int)(a + b));
            return a + b;
        }
    }
}
