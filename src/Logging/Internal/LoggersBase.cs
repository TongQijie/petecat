namespace Petecat.Logging
{
    internal class LoggersBase : ILoggers
    {
        public LoggersBase(ILogger[] loggers)
        {
            Loggers = loggers;
        }

        public ILogger[] Loggers { get; set; }

        public void LogEvent(string category, LoggerLevel loggerLevel, params object[] parameters)
        {
            if (Loggers != null)
            {
                foreach (var logger in Loggers)
                {
                    logger.LogEvent(category, loggerLevel, parameters);
                }
            }
        }
    }
}
