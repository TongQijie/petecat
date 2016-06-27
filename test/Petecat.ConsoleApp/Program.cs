using Petecat.Threading.Tasks;
using Petecat.Console.Outputs;
using Petecat.Console;

namespace Petecat.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var output = new RegularOutput();
            output.Columns.Add(new RegularColumn(0, 5));
            output.Columns.Add(new RegularColumn(1, 20));
            output.Columns.Add(new RegularColumn(2, 40));

            output.OutputColumn(0, "column0");
            output.OutputColumn(1, "hey, dddddddddd");
            output.OutputColumn(2, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            output.OutputColumn(1, "vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv");
            output.OutputColumn(0, "2");
            output.OutputColumn(1, "dddd");

            CommonUtility.WriteNewLine();

            //var taskManager = new BackgroundTaskManager("tasks.config");
            //taskManager.Start();

            //Console.ReadKey();

            //taskManager.Stop();

            CommonUtility.ReadAnyKey();
        }
    }
}
