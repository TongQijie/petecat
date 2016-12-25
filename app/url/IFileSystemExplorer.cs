using System;

namespace Petecat.App.Url
{
    public interface IFileSystemExplorer
    {
        void Filter(string root, string regx, Action<string> action);
    }
}
