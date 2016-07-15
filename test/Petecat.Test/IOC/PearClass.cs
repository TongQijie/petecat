using Petecat.Service.Attributes;

namespace Petecat.Test.IOC
{
    [AutoService(typeof(IPearClass))]
    public class PearClass : IPearClass
    {
        [ServiceMethod]
        public string SayHi(string welcome)
        {
            return welcome;
        }

        [ServiceMethod]
        public BananaClass GetBanana(string name)
        {
            return new BananaClass() { Name = name };
        }

        [ServiceMethod]
        public AppleClass GetApple(BananaClass banana)
        {
            return new AppleClass(banana);
        }
    }
}
