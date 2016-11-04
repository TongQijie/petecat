using Petecat.Console;
using System;

namespace Petecat.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0 && args[0].StartsWith("tcpapplication", StringComparison.OrdinalIgnoreCase))
            {
                new ServiceTcpApplicationTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("tcpclient", StringComparison.OrdinalIgnoreCase))
            {
                new ServiceTcpClientBaseTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("filesystemmonitor", StringComparison.OrdinalIgnoreCase))
            {
                new FileSystemMonitorTest().Run();
                new FileSystemMonitorAnotherTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("container", StringComparison.OrdinalIgnoreCase))
            {
                new DependencyInjection.BaseDirectoryAssemblyContainerTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("cache", StringComparison.OrdinalIgnoreCase))
            {
                new Caching.CacheContainerBaseTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("sfct", StringComparison.OrdinalIgnoreCase))
            {
                new Configuring.StaticFileConfigurerTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
        }
    }
}