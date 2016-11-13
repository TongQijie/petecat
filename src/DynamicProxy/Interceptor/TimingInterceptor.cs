using System.Diagnostics;

using Petecat.DependencyInjection.Attribute;

namespace Petecat.DynamicProxy.Interceptor
{
    [DependencyInjectable(Inference = typeof(ITimingInterceptor), Singleton = true)]
    internal class TimingInterceptor : ITimingInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            invocation.Process();

            stopwatch.Stop();

            Debug.Print("method '{0}' cost {1}ms totally.", invocation.MethodInfo.Name, stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}