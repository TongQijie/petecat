using Petecat.Extending;

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

            var shallowCopy = apple.ShallowCopy<AppleClass>();

            var deepCopy = apple.DeepCopy<AppleClass>();
        }
    }
}