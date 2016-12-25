using System;
using System.IO;

namespace Petecat.App.Url
{
    public interface IFileSystemExplorer
    {
        void Iterate(string folder, Action<FileInfo> fileHandler, Func<DirectoryInfo, bool> folderHandler);
    }
}