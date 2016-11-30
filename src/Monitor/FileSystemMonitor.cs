using System.IO;
using System.Collections.Concurrent;

using Petecat.Extending;
using Petecat.Monitor.Internal;
using Petecat.DependencyInjection.Attribute;

namespace Petecat.Monitor
{
    [DependencyInjectable(Inference = typeof(IFileSystemMonitor), Singleton = true)]
    public class FileSystemMonitor : IFileSystemMonitor
    {
        private ConcurrentDictionary<string, FolderMonitor> _FolderMonitors = new ConcurrentDictionary<string, FolderMonitor>();

        public void Add(object referenceObject, string path, 
            Delegates.FileChangedHandlerDelegate fileChanged,
            Delegates.FileCreatedHandlerDelegate fileCreated,
            Delegates.FileDeletedHandlerDelegate fileDeleted,
            Delegates.FileRenamedHandlerDelegate fileRenamed)
        {
            if (!path.IsFile())
            {
                throw new Errors.InvalidFilePathException(path);
            }

            var fileInfo = new FileInfo(path);
            path = fileInfo.Directory.FullName;
            
            if (!path.IsFolder())
            {
                throw new Errors.InvalidFolderPathException(path);
            }

            FolderMonitor folderMonitor = null;
            if (_FolderMonitors.ContainsKey(path) && !_FolderMonitors[path].ReferencedObjects.Exists(x => x.Equals(referenceObject)))
            {
                folderMonitor = _FolderMonitors[path];
            }
            else
            {
                folderMonitor = new FolderMonitor(path);

                if (!_FolderMonitors.TryAdd(path, folderMonitor))
                {
                    // TODO: throw
                }
            }

            folderMonitor.ReferencedObjects = folderMonitor.ReferencedObjects.Append(referenceObject);

            if (fileChanged != null)
            {
                folderMonitor.FileChanged += fileChanged;
            }
            if (fileCreated != null)
            {
                folderMonitor.FileCreated += fileCreated;
            }
            if (fileDeleted != null)
            {
                folderMonitor.FileDeleted += fileDeleted;
            }
            if (fileRenamed != null)
            {
                folderMonitor.FileRenamed += fileRenamed;
            }

            folderMonitor.Start();
        }

        public void Remove(object referenceObject, string path,
            Delegates.FileChangedHandlerDelegate fileChanged,
            Delegates.FileCreatedHandlerDelegate fileCreated,
            Delegates.FileDeletedHandlerDelegate fileDeleted,
            Delegates.FileRenamedHandlerDelegate fileRenamed)
        {
            if (path.IsFile())
            {
                var fileInfo = new FileInfo(path);
                path = fileInfo.Directory.FullName;
            }

            if (!path.IsFolder())
            {
                throw new Errors.InvalidFolderPathException(path);
            }

            FolderMonitor folderMonitor = null;
            if (_FolderMonitors.ContainsKey(path) && _FolderMonitors[path].ReferencedObjects.Exists(x => x.Equals(referenceObject)))
            {
                folderMonitor = _FolderMonitors[path];
            }
            else
            {
                return;
            }

            folderMonitor.ReferencedObjects = folderMonitor.ReferencedObjects.Remove(x => x.Equals(referenceObject));

            if (fileChanged != null)
            {
                folderMonitor.FileChanged -= fileChanged;
            }
            if (fileCreated != null)
            {
                folderMonitor.FileCreated -= fileCreated;
            }
            if (fileDeleted != null)
            {
                folderMonitor.FileDeleted -= fileDeleted;
            }
            if (fileRenamed != null)
            {
                folderMonitor.FileRenamed -= fileRenamed;
            }

            if (folderMonitor.ReferencedObjects.Length == 0)
            {
                folderMonitor.Stop();
                if (!_FolderMonitors.TryRemove(path, out folderMonitor))
                {
                    // TODO: throw
                }
            }
        }
    }
}
