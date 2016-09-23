namespace Petecat.Logging
{
    public interface ILogger : Collection.IKeyedObject<string>
    {
        void LogEvent(string category, LoggerLevel loggerLevel, params object[] parameters);
    }
}
