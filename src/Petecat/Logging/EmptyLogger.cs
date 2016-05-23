using System;

namespace Petecat.Logging
{
    public class EmptyLogger : ILogger
    {
        public string Key { get { return "EmptyLogger"; } }

        public void LogEvent(string category, LoggerLevel loggerLevel, params object[] parameters)
        {
            // do nothing
        }
    }
}
