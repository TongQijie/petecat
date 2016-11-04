using System;
using Petecat.DynamicProxy;

namespace Petecat.ConsoleApp.DynamicProxy
{
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
