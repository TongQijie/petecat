using Petecat.DependencyInjection.Attribute;
using System;

namespace Petecat.ConsoleApp.DependencyInjection
{
    [DependencyInjectable]
    public class AppleClass : IAppleInterface
    {
        public AppleClass() { }

        public void SayHi(string hi)
        {
            Console.WriteLine("apple: '{0}'.", hi);
        }
    }
}
