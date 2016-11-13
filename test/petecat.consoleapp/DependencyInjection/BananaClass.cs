using Petecat.DependencyInjection.Attribute;

namespace Petecat.ConsoleApp.DependencyInjection
{
    [DependencyInjectable]
    public class BananaClass
    {
        public BananaClass(AppleClass appleClass)
        {
        }

        public void SayHi(string hi)
        {
            Console.ConsoleBridging.WriteLine("banana: '{0}'.", hi);
        }
    }
}
