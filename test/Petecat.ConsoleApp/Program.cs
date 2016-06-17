using Petecat.Threading.Tasks;

namespace Petecat.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var taskManager = new BackgroundTaskManager("tasks.config");
            taskManager.Start();

            System.Console.ReadKey();

            taskManager.Stop();
        }
    }
}
