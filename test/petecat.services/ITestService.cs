using Petecat.Service.Attributes;

namespace Petecat.Services
{
    [ServiceInterface(ServiceName = "test")]
    public interface ITestService
    {
        [ServiceMethod(MethodName = "hi", IsDefaultMethod = true)]
        string Hi();

        [ServiceMethod(MethodName = "")]
        string UpdateInfo(int age);
    }
}
