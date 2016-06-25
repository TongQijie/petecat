﻿using System.Linq;

using Petecat.Collection;

namespace Petecat.Logging
{
    public static class LoggerManager
    {
        public const string AppDomainLoggerName = "AppDomainLogger";

        private static ThreadSafeKeyedObjectCollection<string, ILogger> _Loggers = new ThreadSafeKeyedObjectCollection<string, ILogger>();

        /// <summary>
        /// [Obsolete] replaced by GetLogger()
        /// </summary>
        public static ILogger Get()
        {
            return _Loggers.Get(AppDomainLoggerName, new EmptyLogger());
        }

        /// <summary>
        /// [Obsolete] replaced by GetLogger(string name)
        /// </summary>
        public static ILogger Get(string name)
        {
            return _Loggers.Get(name, Get());
        }

        /// <summary>
        /// [Obsolete] replaced by SetLogger(ILogger logger)
        /// </summary>
        public static void Set(ILogger logger)
        {
            _Loggers.Add(logger);
        }

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
            return _Loggers.Get(name, Get());
        }

        public static ILoggers GetLoggers(params string[] names)
        {
            return new LoggersBase(names.Select(x => Get(x)).ToArray());
        }
    }
}