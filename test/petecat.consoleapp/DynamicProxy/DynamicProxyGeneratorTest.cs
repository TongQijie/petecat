using Petecat.Aop;
using Petecat.DependencyInjection;
using Petecat.DynamicProxy;

namespace Petecat.ConsoleApp.DynamicProxy
{
    class DynamicProxyGeneratorTest
    {
        public void Run()
        {
            var result = -1;

            var durianClass = new DurianClass(new AppleClass());
            durianClass.A();
            durianClass.B(1);
            durianClass.C(1, 2);
            result = durianClass.D();
            result = durianClass.E(1);
            result = durianClass.F(1, 2);

            var bananaClass = DependencyInjector.GetObject<IDynamicProxyGenerator>().CreateProxyObject<BananaClass>(new AppleClass());
            bananaClass.A();
            bananaClass.B(1);
            bananaClass.C(2, 2);
            result = bananaClass.D();
            result = bananaClass.E(1);
            result = bananaClass.F(1, 2);
        }
    }
}
