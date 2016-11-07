using Petecat.Console;
using Petecat.DependencyInjection;
using Petecat.DependencyInjection.Containers;

using System;

namespace Petecat.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DependencyInjector.Setup(new BaseDirectoryAssemblyContainer());

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
            else if (args != null && args.Length > 0 && args[0].StartsWith("bdact", StringComparison.OrdinalIgnoreCase))
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
                new Configuring.StaticFileConfigurerTest().Run1();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("dpgt", StringComparison.OrdinalIgnoreCase))
            {
                new DynamicProxy.DynamicProxyGeneratorTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
        }
    }
}