using Petecat.DependencyInjection.Attribute;
using Petecat.Extending;
using System;
using System.IO;
namespace Files
{
    [DependencyInjectable(Inference = typeof(IDeletiton), Singleton = true)]
    public class Deletion : IDeletiton
    {
        private IFileSystemExplorer _FileSystemExplorer;

        public Deletion(IFileSystemExplorer fileSystemExplorer)
        {
            _FileSystemExplorer = fileSystemExplorer;
        }

        public void Execute(string folder, string[] folders, string[] files)
        {
            _FileSystemExplorer.Iterate(folder, i =>
            {
                if (files.Exists(x => x.EqualsIgnoreCase(i.Name)))
                {
                    Console.WriteLine("delete file: " + i.FullName);
                    File.Delete(i.FullName);
                }
            }, i =>
            {
                if (folders.Exists(x => x.EqualsIgnoreCase(i.Name)))
                {
                    Console.WriteLine("delete folder: " + i.FullName);
                    Directory.Delete(i.FullName, true);
                    return true;
                }

                return false;
            });
        }
    }
}
