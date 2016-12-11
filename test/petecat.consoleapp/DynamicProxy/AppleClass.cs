using Petecat.DynamicProxy;
using Petecat.DependencyInjection.Attribute;

using System;

namespace Petecat.ConsoleApp.DynamicProxy
{
    [DependencyInjectable(Inference = typeof(IAppleInterface), Singleton = true)]
    public class AppleClass : IAppleInterface
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("begin to execute...");
            invocation.Process();
            Console.WriteLine("finished. Result: " + (invocation.ReturnValue == null ? "empty" : invocation.ReturnValue.ToString()));
        }
    }
}