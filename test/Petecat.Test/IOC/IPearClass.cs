using Petecat.Service.Attributes;

namespace Petecat.Test.IoC
{
    [ServiceInterface(ServiceName = "pear")]
    public interface IPearClass
    {
        [ServiceMethod(IsDefaultMethod = true)]
        string SayHi(string welcome);

        [ServiceMethod]
        BananaClass GetBanana(string name);

        [ServiceMethod]
        AppleClass GetApple(BananaClass banana);
    }
}