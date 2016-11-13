using Petecat.DependencyInjection.Attribute;

namespace Petecat.ConsoleApp.DependencyInjection
{
    [DependencyInjectable]
    public class AppleClass : IAppleInterface
    {
        public AppleClass() { }

        public void SayHi(string hi)
        {
            Console.ConsoleBridging.WriteLine("apple: '{0}'.", hi);
        }
    }
}
