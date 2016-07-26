using System.Collections.Generic;
using System.IO;
using System;

using Petecat.Collection;

namespace Petecat.Threading.Watcher
{
    public class FolderWatcher : IKeyedObject<string>
    {
        public FolderWatcher(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(path);
            }

            FullPath = path;

            foreach (var fileInfo in new DirectoryInfo(path).GetFiles())
            {
                _FileWatchers.Add(new FileWatcher(fileInfo.FullName));
            }
        }

        public string FullPath { get; private set; }

        private ThreadSafeKeyedObjectCollection<string, FileWatcher> _FileWatchers = new ThreadSafeKeyedObjectCollection<string, FileWatcher>();

        public FolderWatcher SetFileChangedHandler(string fileName, FileChangedHandlerDelegate handler)
        {
            var fileWatcher = _FileWatchers.Get(fileName, null);
            if (fileWatcher != null)
            {
                fileWatcher.FileChanged += handler;
            }
            else
            {
                if (File.Exists(Path.Combine(FullPath, fileName)))
                {
                    fileWatcher = _FileWatchers.Add(new FileWatcher(Path.Combine(FullPath, fileName)));
                    fileWatcher.FileChanged += handler;
                }
            }

            return this;
        }

        private FileSystemWatcher _Watcher = null;

        public void Start()
        {
            if (_Watcher == null)
            {
                _Watcher = new FileSystemWatcher();
                _Watcher.Path = FullPath;
                _Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                _Watcher.Changed += new FileSystemEventHandler(OnChanged);
            }

            _Watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            if (_Watcher != null)
            {
                _Watcher.Changed -= new FileSystemEventHandler(OnChanged);
                _Watcher.EnableRaisingEvents = false;
                _Watcher = null;
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            var fileWatcher = _FileWatchers.Get(e.Name, null);
            if (fileWatcher != null)
            {
                fileWatcher.FireChanged();
            }
        }

        public string Key { get { return FullPath; } }
    }
}
