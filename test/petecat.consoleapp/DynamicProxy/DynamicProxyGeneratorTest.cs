using Petecat.DependencyInjection;
using Petecat.DynamicProxy;
using Petecat.DynamicProxy.DependencyInjection;

namespace Petecat.ConsoleApp.DynamicProxy
{
    class DynamicProxyGeneratorTest
    {
        public void Run()
        {
            DependencyInjector.Setup(new DynamicProxyAssemblyContainer());

            var result = -1;

            var childBananaClass = DependencyInjector.GetObject<IBananaInterface>();
            result = childBananaClass.F(1, 2);
        }
    }
}
