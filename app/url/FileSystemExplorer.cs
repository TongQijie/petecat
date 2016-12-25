using System;

namespace Petecat.App.Url
{
    public class FileSystemExplorer : IFileSystemExplorer
    {
        public void Filter(string root, string regx, Action<string> action)
        {
        }
    }
}
