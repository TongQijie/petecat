using Petecat.Service.Attributes;

namespace Petecat.Services
{
    [AutoService(typeof(ITestService))]
    public class TestService : ITestService
    {
        public string Hi()
        {
            return "Welcome to access test service.";
        }
    }
}
