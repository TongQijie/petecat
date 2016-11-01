using Petecat.Service;
namespace Petecat.ConsoleApp
{
    public class ServiceTcpApplicationTest
    {
        private TcpApplicationBase _App = null;

        public void Run()
        {
            _App = new TcpApplicationBase();
            _App.Start(12000);
        }
    }
}
