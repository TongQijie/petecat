using Petecat.DependencyInjection.Attribute;
using System;

namespace Petecat.ConsoleApp.DependencyInjection
{
    [DependencyInjectable(Inference = typeof(IDurianInterface))]
    public class DurianClass : IDurianInterface
    {
        public DurianClass(ICherryInterface cherry)
        {
        }

        public void SayHi(string hi)
        {
            Console.WriteLine("durian: '{0}'.", hi);
        }
    }
}
