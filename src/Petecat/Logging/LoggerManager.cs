using System.Linq;

using Petecat.Collection;

namespace Petecat.Logging
{
    public static class LoggerManager
    {
        public const string AppDomainLoggerName = "Global_AppDomainLogger";

        private static ThreadSafeKeyedObjectCollection<string, ILogger> _Loggers = new ThreadSafeKeyedObjectCollection<string, ILogger>();

        public static void SetLogger(ILogger logger)
        {
            _Loggers.Add(logger);
        }

        public static ILogger GetLogger()
        {
            return _Loggers.Get(AppDomainLoggerName, new EmptyLogger());
        }

        public static ILogger GetLogger(string name)
        {
            return _Loggers.Get(name, GetLogger());
        }

        public static ILoggers GetLoggers(params string[] names)
        {
            return new LoggersBase(names.Select(x => GetLogger(x)).ToArray());
        }
    }
}