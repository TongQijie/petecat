using Petecat.Monitor;
using Petecat.Extension;

namespace Petecat.ConsoleApp
{
    class FileSystemMonitorTest
    {
        public void Run()
        {
            FileSystemMonitor.Instance.Add(this, "./".FullPath(),
                (p) => { Console.ConsoleBridging.WriteLine(p); },
                (p) => { Console.ConsoleBridging.WriteLine(p); },
                (p) => { Console.ConsoleBridging.WriteLine(p); },
                (o, n) => { Console.ConsoleBridging.WriteLine(o + n); });
        }
    }
}
