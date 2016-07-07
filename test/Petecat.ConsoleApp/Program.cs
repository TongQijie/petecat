using Petecat.Threading.Tasks;
using Petecat.Console.Outputs;
using Petecat.Console;

namespace Petecat.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Petecat.Restful.ServicesHost();
            app.InitializeServicesHost();

            CommonUtility.ReadAnyKey();
        }
    }
}
