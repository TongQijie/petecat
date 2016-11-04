using Petecat.Configuring;
using Petecat.Data.Formatters;
using Petecat.Extension;
using System.Text;
namespace Petecat.ConsoleApp.Configuring
{
    class StaticFileConfigurerTest
    {
        public void Run()
        {
            var configurer = new StaticFileConfigurer();
            configurer.Append("apple", "./apple.json".FullPath(), "json", typeof(AppleClass));

            while (Console.ConsoleBridging.ReadLine() != "quit")
            {
                Console.ConsoleBridging.WriteLine(new JsonFormatter().WriteString(configurer.GetValue<AppleClass>("apple"), Encoding.UTF8));
            }

            configurer.Remove("apple");
        }
    }
}
