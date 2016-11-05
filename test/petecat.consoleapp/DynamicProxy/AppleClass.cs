using System;
using Petecat.DynamicProxy;
using Petecat.DependencyInjection.Attributes;

namespace Petecat.ConsoleApp.DynamicProxy
{
    [DependencyInjectable(Sington = true)]
    public class AppleClass : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.ConsoleBridging.WriteLine("begin to execute...");
            invocation.Process();
            Console.ConsoleBridging.WriteLine("finished. Result: " + (invocation.ReturnValue == null ? "empty" : invocation.ReturnValue.ToString()));
        }
    }
}
