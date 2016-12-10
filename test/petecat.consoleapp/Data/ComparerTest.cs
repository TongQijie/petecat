using Petecat.Data;
using Petecat.DependencyInjection;
namespace Petecat.ConsoleApp.Data
{
    public class ComparerTest
    {
        public void Run()
        {
            var comparer = DependencyInjector.GetObject<IComparer>();
            var rst = comparer.Compare(() => true.ToString().CompareTo(false.ToString()));
        }
    }
}