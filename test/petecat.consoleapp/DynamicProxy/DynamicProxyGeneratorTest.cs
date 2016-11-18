using Petecat.DependencyInjection;
using Petecat.DependencyInjection.Containers;
using Petecat.DynamicProxy;
using Petecat.DynamicProxy.DependencyInjection;

namespace Petecat.ConsoleApp.DynamicProxy
{
    class DynamicProxyGeneratorTest
    {
        public void Run()
        {
            DependencyInjector.GetContainer<BaseDirectoryAssemblyContainer>().RegisterAssemblies<DynamicProxyAssemblyInfo>();

            var result = -1;

            var childBananaClass = DependencyInjector.GetObject<IBananaInterface>();
            result = childBananaClass.F(1, 2);
        }
    }
}
