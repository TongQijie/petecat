using Petecat.DependencyInjection.Attribute;
namespace Petecat.ConsoleApp.DependencyInjection
{
    [DependencyInjectable(Inference = typeof(IFilbertInterface), Singleton = true)]
    public class FilbertClass : IFilbertInterface
    {
        public void SayHi(string hi)
        {
            Count++;
            Console.ConsoleBridging.WriteLine("filbert: '{0}'. Count = '{1}'", hi, Count);
        }

        public int Count { get; private set; }
    }
}
