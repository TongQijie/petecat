using Petecat.DynamicProxy;
using Petecat.DependencyInjection.Attributes;

namespace Petecat.ConsoleApp.DynamicProxy
{
    [DependencyInjectable(Inference = typeof(IAppleInterface), Singleton = true)]
    public class AppleClass : IAppleInterface
    {
        public void Intercept(IInvocation invocation)
        {
            Console.ConsoleBridging.WriteLine("begin to execute...");
            invocation.Process();
            Console.ConsoleBridging.WriteLine("finished. Result: " + (invocation.ReturnValue == null ? "empty" : invocation.ReturnValue.ToString()));
        }
    }
}