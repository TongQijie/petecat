using System;
using System.IO;
using System.Text;
using System.Linq;

namespace Petecat.Logging.Loggers
{
    public class FileLogger : ILogger
    {
        public FileLogger(string key, string logDirectory)
        {
            Key = key;
            LogDirectory = logDirectory;
        }

        public string Key { get; private set; }

        public string LogDirectory { get; private set; }

        public void LogEvent(string category, LoggerLevel loggerLevel, params object[] parameters)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}|{1,-5}|{2}|", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), loggerLevel, category);
            if (parameters != null)
            {
                foreach (var parameter in parameters.Where(x => x != null))
                {
                    if (parameter is Exception)
                    {
                        stringBuilder.Append(new ExceptionWrapper(parameter as Exception).ToString());
                    }
                    else
                    {
                        stringBuilder.Append(parameter.ToString());
                    }
                    stringBuilder.Append("|");
                }
            }

            Flush(stringBuilder.ToString().TrimEnd('|'));
        }

        private object _FlushLocker = new object();

        public void Flush(string text)
        {
            lock (_FlushLocker)
            {
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }

                var fileName = string.Format("{0}.log", DateTime.Now.ToString("yyyy-MM-dd"));
                try
                {
                    using (var sw = new StreamWriter(Path.Combine(LogDirectory, fileName), true, Encoding.UTF8))
                    {
                        sw.WriteLine(text);
                    }
                }
                catch (Exception) { }
            }
        }
    }
}