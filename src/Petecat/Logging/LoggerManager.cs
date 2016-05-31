using Petecat.Collection;

namespace Petecat.Logging
{
    public static class LoggerManager
    {
        public const string AppDomainLoggerName = "AppDomainLogger";

        private static ThreadSafeKeyedObjectCollection<string, ILogger> _Loggers = new ThreadSafeKeyedObjectCollection<string, ILogger>();

        public static ILogger Get(string name)
        {
            return _Loggers.Get(name, new Loggers.EmptyLogger());
        }

        public static ILogger Get()
        {
            return _Loggers.Get(AppDomainLoggerName, new Loggers.EmptyLogger());
        }

        public static void Set(ILogger logger)
        {
            _Loggers.Add(logger);
        }
    }
}