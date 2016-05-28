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

            LoggerManager.Get("test").LogEvent(Assembly.GetExecutingAssembly().FullName, LoggerLevel.Error, "this is a error.");
            LoggerManager.Get("test").LogEvent(Assembly.GetExecutingAssembly().FullName, LoggerLevel.Info, "this is a info.", new ExceptionWrapper(new Exception("ffffffff", new FormatException("ddfdgadg"))));
        }

        private void Setup()
        {
            LoggerManager.Set(new FileLogger("test", "test.log"));
        }
    }
}
