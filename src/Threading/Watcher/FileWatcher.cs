using System;
using System.IO;

using Petecat.Collection;

namespace Petecat.Threading.Watcher
{
    public class FileWatcher : IKeyedObject<string>
    {
        public FileWatcher(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            var fileInfo = new FileInfo(path);
            FullPath = fileInfo.FullName;
            Name = fileInfo.Name;
            LastWriteTime = fileInfo.LastWriteTime;
        }

        public string FullPath { get; private set; }

        public string Name { get; set; }

        public event FileChangedHandlerDelegate FileChanged = null;

        public DateTime LastWriteTime { get; private set; }

        public void FireChanged()
        {
            try
            {
                var lastWriteTime = File.GetLastWriteTime(FullPath);

                if (lastWriteTime != LastWriteTime && FileChanged != null)
                {
                    LastWriteTime = lastWriteTime;

                    // sleep 100ms to ensure file is closed
                    ThreadBridging.Sleep(100);

                    FileChanged(this);
                }
            }
            catch (Exception e)
            {
                //Logging.LoggerManager.GetLogger().LogEvent("FileWatcher", Logging.LoggerLevel.Error, FullPath, e);
            }
        }

        public string Key { get { return Name; } }
    }
}
