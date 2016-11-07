using Petecat.Monitor;
using Petecat.Extension;
using Petecat.DependencyInjection;

namespace Petecat.ConsoleApp.Monitor
{
    class FileSystemMonitorTest
    {
        public void Run()
        {
            var monitor = DependencyInjector.GetObject<IFileSystemMonitor>();

            monitor.Add(this, "./".FullPath(),
                (p) => { Console.ConsoleBridging.WriteLine("1 " + "e " + p); },
                (p) => { Console.ConsoleBridging.WriteLine("1 " + "c " + p); },
                (p) => { Console.ConsoleBridging.WriteLine("1 " + "d " + p); },
                (o, n) => { Console.ConsoleBridging.WriteLine("1 " + "r " + o + " " + n); });
        }
    }

    class FileSystemMonitorAnotherTest
    {
        public void Run()
        {
            var monitor = DependencyInjector.GetObject<IFileSystemMonitor>();

            monitor.Add(this, "./".FullPath(),
                (p) => { Console.ConsoleBridging.WriteLine("2 " + "e " + p); },
                (p) => { Console.ConsoleBridging.WriteLine("2 " + "c " + p); },
                (p) => { Console.ConsoleBridging.WriteLine("2 " + "d " + p); },
                (o, n) => { Console.ConsoleBridging.WriteLine("2 " + "r " + o + " " + n); });
        }
    }
}
