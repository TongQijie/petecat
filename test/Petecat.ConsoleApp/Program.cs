﻿using Petecat.Console;
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

            if (args != null && args.Length > 0 && args[0].StartsWith("fsmt", StringComparison.OrdinalIgnoreCase))
            {
                new Monitor.FileSystemMonitorTest().Run();
                new Monitor.FileSystemMonitorAnotherTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("bdact", StringComparison.OrdinalIgnoreCase))
            {
                new DependencyInjection.BaseDirectoryAssemblyContainerTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("bdcct", StringComparison.OrdinalIgnoreCase))
            {
                new DependencyInjection.BaseDirectoryConfigurableContainerTest().Run();
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
            else if (args != null && args.Length > 0 && args[0].StartsWith("jft", StringComparison.OrdinalIgnoreCase))
            {
                new Formatter.JsonFormatterTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("flt", StringComparison.OrdinalIgnoreCase))
            {
                new Logging.FileLoggerTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("tcplot", StringComparison.OrdinalIgnoreCase))
            {
                new Network.Socket.TcpListenerObjectTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("dcct", StringComparison.OrdinalIgnoreCase))
            {
                new EntityFramework.DatabaseCommandConfigurationTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("rt", StringComparison.OrdinalIgnoreCase))
            {
                new Data.ReplicatorTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
            else if (args != null && args.Length > 0 && args[0].StartsWith("ct", StringComparison.OrdinalIgnoreCase))
            {
                new Data.ComparerTest().Run();
                ConsoleBridging.ReadAnyKey();
            }
        }
    }
}