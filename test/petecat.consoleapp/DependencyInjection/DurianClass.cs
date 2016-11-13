using Petecat.DependencyInjection.Attribute;

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
            Console.ConsoleBridging.WriteLine("durian: '{0}'.", hi);
        }
    }
}
