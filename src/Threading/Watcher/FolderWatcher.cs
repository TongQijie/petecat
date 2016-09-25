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

        public string Key { get { return FullPath; } }

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

        public FolderWatcher SetFileCreatedHandler(FileCreatedHandlerDelegate handler)
        {
            FileCreated = handler;
            return this;
        }

        public FolderWatcher SetFileDeletedHandler(FileDeletedHandlerDelegate handler)
        {
            FileDeleted = handler;
            return this;
        }

        public FolderWatcher SetFileRenamedHandler(FileRenamedHandlerDelegate handler)
        {
            FileRenamed = handler;
            return this;
        }

        private FileCreatedHandlerDelegate FileCreated = null;

        private FileDeletedHandlerDelegate FileDeleted = null;

        private FileRenamedHandlerDelegate FileRenamed = null;

        private FileSystemWatcher _Watcher = null;

        public void Start()
        {
            if (_Watcher == null)
            {
                _Watcher = new FileSystemWatcher();
                _Watcher.Path = FullPath;
                _Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                _Watcher.Changed += new FileSystemEventHandler(OnChanged);
                _Watcher.Created += new FileSystemEventHandler(OnCreated);
                _Watcher.Deleted += new FileSystemEventHandler(OnDeleted);
                _Watcher.Renamed += new RenamedEventHandler(OnRenamed);
            }

            _Watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            if (_Watcher != null)
            {
                _Watcher.Changed -= new FileSystemEventHandler(OnChanged);
                _Watcher.Created -= new FileSystemEventHandler(OnCreated);
                _Watcher.Deleted -= new FileSystemEventHandler(OnDeleted);
                _Watcher.Renamed -= new RenamedEventHandler(OnRenamed);
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

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            FileAttributes fileAttributes;
            try
            {
                fileAttributes = File.GetAttributes(e.FullPath);
            }
            catch (Exception) { return; }

            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                // do nothing
            }
            else
            {
                if (FileCreated != null)
                {
                    FileCreated.Invoke(this, e.Name);
                }
            }
        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            if (FileDeleted != null)
            {
                FileDeleted.Invoke(this, e.Name);
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            var fileAttributes = File.GetAttributes(e.FullPath);
            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                // do nothing
            }
            else
            {
                if (FileRenamed != null)
                {
                    FileRenamed.Invoke(this, e.OldName, e.Name);
                }
            }
        }

        
    }
}
