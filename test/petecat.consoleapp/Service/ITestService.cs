using Petecat.Service.Attributes;
namespace Petecat.ConsoleApp.Service
{
    [ServiceInterface(ServiceName = "test")]
    public interface ITestService
    {
        [ServiceMethod(MethodName = "sayhi", IsDefaultMethod = true)]
        ServiceCustomResponse SayHi(ServiceCustomRequest request);
    }
}
