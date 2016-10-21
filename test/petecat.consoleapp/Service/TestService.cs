using Petecat.Service.Attributes;
namespace Petecat.ConsoleApp.Service
{
    [AutoService(typeof(ITestService))]
    public class TestService : ITestService
    {
        public ServiceCustomResponse SayHi(ServiceCustomRequest request)
        {
            return new ServiceCustomResponse() { Result = "hey" };
        }
    }
}
