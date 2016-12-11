using Petecat.DependencyInjection.Attribute;
using System;

namespace Petecat.ConsoleApp.DependencyInjection
{
    [DependencyInjectable(Inference = typeof(IFilbertInterface), Singleton = true)]
    public class FilbertClass : IFilbertInterface
    {
        public void SayHi(string hi)
        {
            Count++;
            Console.WriteLine("filbert: '{0}'. Count = '{1}'", hi, Count);
        }

        public int Count { get; private set; }
    }
}
