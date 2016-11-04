using Petecat.Caching;
using Petecat.Data.Formatters;
using Petecat.Extension;
using System.Text;
namespace Petecat.ConsoleApp.Caching
{
    class CacheContainerBaseTest
    {
        private ICacheContainer _Container = null;

        public ICacheContainer Container { get { return _Container ?? (_Container = new CacheContainerBase()); } }

        public void Run()
        {
            Container.Add(new JsonFileCacheItem("apple", "./apple.json".FullPath(), typeof(AppleClass)));
            var appleClass = Container.Get("apple").GetValue() as AppleClass;
            Console.ConsoleBridging.WriteLine(new JsonFormatter().WriteString(appleClass, Encoding.UTF8));

            while (true)
            {
                Console.ConsoleBridging.WriteLine("hit any key to set dirty...");
                Console.ConsoleBridging.ReadAnyKey();
                Container.Get("apple").IsDirty = true;
                appleClass = Container.Get("apple").GetValue() as AppleClass;
                Console.ConsoleBridging.WriteLine(new JsonFormatter().WriteString(appleClass, Encoding.UTF8));
            }
        }
    }
}
