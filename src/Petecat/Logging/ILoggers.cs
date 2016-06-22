namespace Petecat.Logging
{
    public interface ILoggers
    {
        ILogger[] Loggers { get; }

        void LogEvent(string category, LoggerLevel loggerLevel, params object[] parameters);
    }
}
