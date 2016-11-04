using Petecat.DependencyInjection;
using Petecat.DependencyInjection.Containers;

namespace Petecat.ConsoleApp.DependencyInjection
{
    class BaseDirectoryAssemblyContainerTest
    {
        public void Run()
        {
            DependencyInjector.Setup(new BaseDirectoryAssemblyContainer());

            var appleClass = DependencyInjector.GetObject(typeof(AppleClass)) as AppleClass;
            if (appleClass != null)
            {
                appleClass.SayHi("public AppleClass() { }");
            }

            var bananaClass = DependencyInjector.GetObject(typeof(BananaClass)) as BananaClass;
            if (bananaClass != null)
            {
                bananaClass.SayHi("public BananaClass(AppleClass appleClass)");
            }

            var cherryInterface = DependencyInjector.GetObject(typeof(ICherryInterface)) as ICherryInterface;
            if (cherryInterface != null)
            {
                cherryInterface.SayHi("[DependencyInjectable(Inference = typeof(ICherryInterface))]");
            }

            var durianInterface = DependencyInjector.GetObject(typeof(IDurianInterface)) as IDurianInterface;
            if (durianInterface != null)
            {
                durianInterface.SayHi("public DurianClass(ICherryInterface cherry)");
            }

            var filbertInterface1 = DependencyInjector.GetObject(typeof(IFilbertInterface)) as IFilbertInterface;
            if (filbertInterface1 != null)
            {
                filbertInterface1.SayHi("filbertInterface1");
            }

            var filbertInterface2 = DependencyInjector.GetObject(typeof(IFilbertInterface)) as IFilbertInterface;
            if (filbertInterface2 != null)
            {
                filbertInterface2.SayHi("filbertInterface2");
            }
        }
    }
}
