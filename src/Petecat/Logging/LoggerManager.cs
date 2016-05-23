using Petecat.Collection;

namespace Petecat.Logging
{
    public static class LoggerManager
    {
        private static ThreadSafeKeyedObjectCollection<string, ILogger> _Loggers = new ThreadSafeKeyedObjectCollection<string, ILogger>();

        public static ILogger Get(string name)
        {
            return _Loggers.Get(name, new EmptyLogger());
        }

        public static ILogger Get()
        {
            return _Loggers.Get("AppDomainLogger", new EmptyLogger());
        }

        public static void Set(ILogger logger)
        {
            _Loggers.Add(logger);
        }
    }
}