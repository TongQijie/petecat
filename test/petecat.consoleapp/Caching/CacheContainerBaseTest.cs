using Petecat.Caching;
using Petecat.Formatter;
using Petecat.Extending;
using System.Text;
using System;

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
            Console.WriteLine(new JsonFormatter().WriteString(appleClass, Encoding.UTF8));

            while (true)
            {
                Console.WriteLine("hit any key to set dirty...");
                Console.ReadKey();
                Container.Get("apple").IsDirty = true;
                appleClass = Container.Get("apple").GetValue() as AppleClass;
                Console.WriteLine(new JsonFormatter().WriteString(appleClass, Encoding.UTF8));
            }
        }
    }
}
