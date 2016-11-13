using Petecat.DependencyInjection.Attribute;

namespace Petecat.ConsoleApp.DependencyInjection
{
    [DependencyInjectable(Inference = typeof(ICherryInterface))]
    public class CherryClass : ICherryInterface
    {
        public void SayHi(string hi)
        {
            Console.ConsoleBridging.WriteLine("cherry: '{0}'.", hi);
        }
    }
}
