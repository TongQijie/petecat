using Petecat.Aop;
using Petecat.DependencyInjection;
using Petecat.DynamicProxy;
using System;
using System.Linq;
using Petecat.Extension;

namespace Petecat.ConsoleApp.DynamicProxy
{
    class DynamicProxyGeneratorTest
    {
        public void Run()
        {
            var result = -1;

            //var bananaClass = new BananaClass();
            //bananaClass.F(1, 2);
            
            //var durianClass = new DurianClass(new AppleClass());
            //result = durianClass.F(1, 2);

            var childBananaClass = DependencyInjector.GetObject<IDynamicProxyGenerator>().CreateProxyObject<BananaClass>();
            //bananaClass.A();
            //bananaClass.B(1);
            //bananaClass.C(2, 2);
            //result = bananaClass.D();
            //result = bananaClass.E(1);
            result = childBananaClass.F(1, 2);
        }
    }
}
