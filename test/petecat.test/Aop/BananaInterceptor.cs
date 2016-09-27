using Petecat.Aop;
using Petecat.Console;
namespace Petecat.Test.Aop
{
    public class BananaInterceptor : IAopInterceptor
    {
        public void Intercept(IAopInvocation invocation)
        {
            ConsoleBridging.WriteLine("started...");
            invocation.Process();
            ConsoleBridging.WriteLine("stopped...");
        }
    }
}
