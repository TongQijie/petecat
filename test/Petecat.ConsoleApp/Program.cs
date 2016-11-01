using Petecat.Console;
using System;

namespace Petecat.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0 && args[0].StartsWith("s", StringComparison.OrdinalIgnoreCase))
            {
                new ServiceTcpApplicationTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("c", StringComparison.OrdinalIgnoreCase))
            {
                new ServiceTcpClientBaseTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("m", StringComparison.OrdinalIgnoreCase))
            {
                new FileSystemMonitorTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
        }
    }
}