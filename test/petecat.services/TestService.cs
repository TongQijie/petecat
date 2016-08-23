using Petecat.Service.Attributes;
using Petecat.Test.IoC;

namespace Petecat.Services
{
    [AutoService(typeof(ITestService))]
    public class TestService : ITestService
    {
        private AppleClass _AppleClass;

        public TestService(AppleClass appleClass)
        {
            _AppleClass = appleClass;
        }

        public string Hi()
        {
            return "Welcome to access test service.";
        }

        public string UpdateInfo(int age)
        {
            return age.ToString();
        }
    }
}
