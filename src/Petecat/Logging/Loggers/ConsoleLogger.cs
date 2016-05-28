using System.Text;
using System.Linq;
using System;

namespace Petecat.Logging.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger(string key)
        {
            Key = key;
        }

        public string Key { get; private set; }

        public void LogEvent(string category, LoggerLevel loggerLevel, params object[] parameters)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}|{1,-5}|{2}|", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), loggerLevel, category);
            if (parameters != null)
            {
                foreach (var parameter in parameters.Where(x => x != null))
                {
                    stringBuilder.Append(parameter.ToString());
                    stringBuilder.Append("|");
                }
            }

            Console.WriteLine(stringBuilder.ToString().TrimEnd('|'));
        }
    }
}
