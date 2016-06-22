using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Logging.Loggers;
using Petecat.Logging;

using System;
using System.Reflection;

namespace Petecat.Test.Logging.Loggers
{
    [TestClass]
    public class FileLoggerTest
    {
        [TestMethod]
        public void LogEvent()
        {
            Setup();

            LoggerManager.GetLoggers("test01", "test02").LogEvent(Assembly.GetExecutingAssembly().FullName, LoggerLevel.Error, "this is a error.");
            LoggerManager.GetLoggers("test01", "test02").LogEvent(Assembly.GetExecutingAssembly().FullName, LoggerLevel.Info, "this is a info.", new ExceptionWrapper(new Exception("ffffffff", new FormatException("ddfdgadg"))));
        }

        private void Setup()
        {
            LoggerManager.Set(new FileLogger("test01", "test01.log"));
            LoggerManager.Set(new FileLogger("test02", "test02.log"));
        }
    }
}
