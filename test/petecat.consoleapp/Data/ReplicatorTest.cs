using Petecat.Data;
using Petecat.DependencyInjection;
namespace Petecat.ConsoleApp.Data
{
    public class ReplicatorTest
    {
        public void Run()
        {
            var apple = new AppleClass()
            {
                B = "apple class",
                Banana = new BananaClass() { B = 1 },
            };

            var shallowCopy = DependencyInjector.GetObject<IReplicator>().ShallowCopy<AppleClass>(apple);

            var deepCopy = DependencyInjector.GetObject<IReplicator>().DeepCopy<AppleClass>(apple);
        }
    }
}