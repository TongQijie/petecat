using Petecat.Extension;
using Petecat.Threading;
using Petecat.DependencyInjection.Attributes;

using System;
using System.IO;
using System.Text;
using System.Collections.Concurrent;

namespace Petecat.Logging
{
    [DependencyInjectable(Inference = typeof(IFileLogger), Singleton = true)]
    public class FileLogger : IFileLogger
    {
        public FileLogger()
        {
            Folder = "./".FullPath();
        }

        public string Folder { get; private set; }

        public void LogEvent(string category, Severity severity, params object[] parameters)
        {
            _Items.Enqueue(new Item(category, severity, parameters));

            if (!_IsAlive)
            {
                StartThread();
            }
        }

        private ConcurrentQueue<Item> _Items = new ConcurrentQueue<Item>();

        private bool _IsAlive = false;

        private object StartThreadLocker = new object();

        private void StartThread()
        {
            if (!_IsAlive)
            {
                lock (StartThreadLocker)
                {
                    if (!_IsAlive)
                    {
                        new ThreadObject(() =>
                        {
                            while (true)
                            {
                                while (_Items.Count > 0)
                                {
                                    Item item;
                                    if (_Items.TryDequeue(out item))
                                    {
                                        Flush(item);
                                    }
                                }

                                ThreadBridging.Sleep(1000);
                            }
                        }).Start();

                        _IsAlive = true;
                    }
                }
            }
        }

        private void Flush(Item item)
        {
            try
            {
                if (!Directory.Exists(Folder))
                {
                    Directory.CreateDirectory(Folder);
                }

                var fileName = string.Format("{0}.log", DateTime.Now.ToString("yyyy-MM-dd"));
                using (var sw = new StreamWriter(Path.Combine(Folder, fileName), true, Encoding.UTF8))
                {
                    sw.WriteLine(item.ToString());
                }
            }
            catch (Exception) { }
        }

        class Item
        {
            public Item(string category, Severity severity, object[] parameters)
            {
                Category = category;
                Severity = severity;
                Parameters = parameters;
                Timestamp = DateTime.Now;
            }

            public string Category { get; set; }

            public Severity Severity { get; set; }

            public object[] Parameters { get; set; }

            public DateTime Timestamp { get; private set; }

            public override string ToString()
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("{0}|{1,-6}|{2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), Severity, Category);
                stringBuilder.AppendLine();
                if (Parameters != null)
                {
                    foreach (var parameter in Parameters.Where(x => x != null))
                    {
                        if (parameter is Exception)
                        {
                            stringBuilder.Append(new ExceptionWrapper(parameter as Exception).ToString());
                        }
                        else
                        {
                            stringBuilder.Append(parameter.ToString());
                        }
                        stringBuilder.AppendLine();
                    }
                }

                return stringBuilder.ToString();
            }
        }
    }
}