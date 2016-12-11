using Petecat.DependencyInjection;
using Petecat.DependencyInjection.Containers;

using Petecat.Extending;
using System;

namespace Petecat.ConsoleApp.DependencyInjection
{
    public class BaseDirectoryConfigurableContainerTest
    {
        public void Run()
        {
            DependencyInjector.Setup(new BaseDirectoryConfigurableContainer("./DependencyInjection/apple.json"));

            while (Console.ReadLine() != "quit")
            {
                var apple1 = DependencyInjector.GetObject("apple") as GrapeClass;
                Console.WriteLine("count = {0}", apple1.Count);
                apple1.Count++;
                var apple2 = DependencyInjector.GetObject("apple") as GrapeClass;
                Console.WriteLine("count = {0}", apple2.Count);

                var banana1 = DependencyInjector.GetObject("banana") as GrapeClass;
                Console.WriteLine("count = {0}", banana1.Count);
                banana1.Count++;
                var banana2 = DependencyInjector.GetObject("banana") as GrapeClass;
                Console.WriteLine("count = {0}", banana2.Count);

                var cherry = DependencyInjector.GetObject("cherry") as GrapeClass;
                Console.WriteLine("count = {0}", cherry.Count);

                var durian = DependencyInjector.GetObject("durian") as GrapeClass;
                Console.WriteLine("count = {0}", durian.Count);

                var filbert = DependencyInjector.GetObject("filbert") as HawClass;
                Console.WriteLine("count = {0}", filbert.GrapeClass.Count);

                var grape = DependencyInjector.GetObject("grape") as HawClass;
                Console.WriteLine("count = {0}", grape.GrapeClass.Count);

                var haw = DependencyInjector.GetObject("haw") as HawClass;
                Console.WriteLine("count = {0}", haw.GrapeClass.Count);

                var kiwifruit = DependencyInjector.GetObject("kiwifruit") as HawClass;
                Console.WriteLine("count = {0}", kiwifruit.GrapeClass.Count);
            }
        }
    }
}
