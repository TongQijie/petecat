using Petecat.Configuring;
using Petecat.Formatter;
using Petecat.DependencyInjection;
using Petecat.Extending;
using System.Text;
namespace Petecat.ConsoleApp.Configuring
{
    class StaticFileConfigurerTest
    {
        public void Run()
        {
            var configurer = DependencyInjector.GetObject<IStaticFileConfigurer>();
            configurer.Append("apple", "./apple.json".FullPath(), "json", typeof(AppleClass));

            while (Console.ConsoleBridging.ReadLine() != "quit")
            {
                Console.ConsoleBridging.WriteLine(new JsonFormatter().WriteString(configurer.GetValue<AppleClass>("apple"), Encoding.UTF8));
            }

            configurer.Remove("apple");
        }

        public void Run1()
        {
            var configurer = DependencyInjector.GetObject<IStaticFileConfigurer>();

            while (Console.ConsoleBridging.ReadLine() != "quit")
            {
                Console.ConsoleBridging.WriteLine(new JsonFormatter().WriteString(configurer.GetValue<IBananaInterface>("banana"), Encoding.UTF8));
            }

            configurer.Remove("banana");
        }
    }
}
