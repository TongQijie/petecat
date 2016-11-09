using Petecat.DependencyInjection;
using Petecat.Logging;
using System;
namespace Petecat.ConsoleApp.Logging
{
    public class FileLoggerTest
    {
        public void Run()
        {
            DependencyInjector.GetObject<IFileLogger>().LogEvent("FileLoggerTest", Severity.Debug, "hi, man!");
            DependencyInjector.GetObject<IFileLogger>().LogEvent("FileLoggerTest", Severity.Debug, "hi, man!", "hello, world!");
            DependencyInjector.GetObject<IFileLogger>().LogEvent("FileLoggerTest", Severity.Debug, new Exception("debug exception", new FormatException("inner exception.")));
        }
    }
}
