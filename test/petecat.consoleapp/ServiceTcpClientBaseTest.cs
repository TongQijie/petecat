using Petecat.Console;
using Petecat.ConsoleApp.Service;
using Petecat.Service.Client;
namespace Petecat.ConsoleApp
{
    public class ServiceTcpClientBaseTest
    {
        public void Run()
        {
            var response = new ServiceTcpClientBase("test").Call<ServiceCustomResponse>(new ServiceCustomRequest() { Id = 1 });
            ConsoleBridging.WriteLine(response.Result);
        }
    }
}
