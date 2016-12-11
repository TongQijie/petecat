using Petecat.DependencyInjection.Attribute;
using System;

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
            Console.WriteLine("banana: '{0}'.", hi);
        }
    }
}
