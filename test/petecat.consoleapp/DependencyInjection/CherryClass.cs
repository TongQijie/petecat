using Petecat.DependencyInjection.Attribute;
using System;

namespace Petecat.ConsoleApp.DependencyInjection
{
    [DependencyInjectable(Inference = typeof(ICherryInterface))]
    public class CherryClass : ICherryInterface
    {
        public void SayHi(string hi)
        {
            Console.WriteLine("cherry: '{0}'.", hi);
        }
    }
}
