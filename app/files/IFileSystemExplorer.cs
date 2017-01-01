using System;
using System.IO;

namespace Files
{
    public interface IFileSystemExplorer
    {
        void Iterate(string folder, Action<FileInfo> fileHandler, Func<DirectoryInfo, bool> folderHandler);
    }
}