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

            var apples = new AppleClass[]
            {
                new AppleClass() { B = "apple1", Banana = new BananaClass() { B = 1 } },
                new AppleClass() { B = "apple2", Banana = new BananaClass() { B = 2 } },
            };

            var shallowCopies = apples.ShallowCopy<AppleClass[]>();

            var deepCopies = apples.DeepCopy<AppleClass[]>();
        }
    }
}