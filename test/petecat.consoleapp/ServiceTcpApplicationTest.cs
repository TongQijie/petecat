using Petecat.Service;
namespace Petecat.ConsoleApp
{
    public class ServiceTcpApplicationTest
    {
        private ServiceTcpApplication _App = null;

        public void Run()
        {
            _App = new ServiceTcpApplication();
            _App.Start(12000);
        }
    }
}
