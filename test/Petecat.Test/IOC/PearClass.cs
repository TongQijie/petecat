using Petecat.Service.Attributes;

namespace Petecat.Test.IoC
{
    [AutoService(typeof(IPearClass))]
    public class PearClass : IPearClass
    {
        public string SayHi(string welcome)
        {
            return welcome;
        }

        public BananaClass GetBanana(string name)
        {
            return new BananaClass() { Name = name };
        }

        public AppleClass GetApple(BananaClass banana)
        {
            return new AppleClass(banana);
        }
    }
}