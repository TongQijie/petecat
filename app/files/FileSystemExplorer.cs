using Petecat.DependencyInjection.Attribute;
using Petecat.Extending;
using System;
using System.IO;

namespace Files
{
    [DependencyInjectable(Inference = typeof(IFileSystemExplorer), Singleton = true)]
    public class FileSystemExplorer : IFileSystemExplorer
    {
        public void Iterate(string folder, Action<FileInfo> fileHandler, Func<DirectoryInfo, bool> folderHandler)
        {
            if (!folder.IsFolder())
            {
                throw new Exception(string.Format("folder '{0}' is not valid.", folder));
            }

            var directoryInfo = new DirectoryInfo(folder);

            foreach (var info in directoryInfo.GetFiles())
            {
                fileHandler(info);
            }

            foreach (var info in directoryInfo.GetDirectories())
            {
                if (!folderHandler(info))
                {
                    Iterate(info.FullName, fileHandler, folderHandler);
                }
            }
        }
    }
}
