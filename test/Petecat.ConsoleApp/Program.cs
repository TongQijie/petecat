using Petecat.Threading.Tasks;
using Petecat.Console.Outputs;
using Petecat.Console;
using Petecat.IOC;

namespace Petecat.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new DefaultContainer();
            container.Register("./configuration/container.config");

            var apple = container.Resolve("apple");
            var banana = container.Resolve("banana");
            var another = container.Resolve("another-apple");

            CommonUtility.ReadAnyKey();
        }
    }
}
