using Petecat.DependencyInjection;
using Petecat.DynamicProxy;

namespace Petecat.ConsoleApp.DynamicProxy
{
    class DynamicProxyGeneratorTest
    {
        public void Run()
        {
            var bananaClass = DependencyInjector.GetObject<IDynamicProxyGenerator>().CreateProxyObject<BananaClass>(new AppleClass());
            bananaClass.A();
            bananaClass.B(1);
            bananaClass.C(2);
        }
    }
}
