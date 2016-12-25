using System.IO;
using System.Text;

using Petecat.Extending;
using Petecat.Configuring;
using Petecat.DependencyInjection.Attribute;

namespace Petecat.App.Url
{
    [DependencyInjectable(Inference = typeof(IUrlReplacement), Singleton = true)]
    public class UrlReplacement : IUrlReplacement
    {
        private IFileSystemExplorer _FileSystemExplorer;

        private IStaticFileConfigurer _StaticFileConfigurer;

        public UrlReplacement(IFileSystemExplorer fileSystemExplorer, IStaticFileConfigurer staticFileConfigurer)
        {
            _FileSystemExplorer = fileSystemExplorer;
            _StaticFileConfigurer = staticFileConfigurer;
        }

        public void Execute(string folder, string value, string replacement)
        {
            var replacementConfiguration = _StaticFileConfigurer.GetValue<IUrlConfiguration>().Replacement ?? new ReplacementConfiguration();

            var fileExtensions = replacementConfiguration.FileExtensions ?? new string[0];

            var ignoreFolders = replacementConfiguration.IgnoreFolders ?? new string[0];

            _FileSystemExplorer.Iterate(folder, i =>
            {
                if (fileExtensions.Exists(x => x.EqualsWith(i.Extension)))
                {
                    Replace(i.FullName, value, replacement);
                }
            }, i =>
            {
                return ignoreFolders.Exists(x => x.EqualsWith(i.Name));
            });
        }

        private void Replace(string path, string value, string replacement)
        {
            var exists = false;

            var text = "";
            using (var inputStream = new StreamReader(path, Encoding.UTF8))
            {
                text = inputStream.ReadToEnd();
            }

            if (text.Contains(value))
            {
                exists = true;
            }

            if (!exists)
            {
                return;
            }

            using (var outputStream = new StreamWriter(path, false, Encoding.UTF8))
            {
                outputStream.Write(text.Replace(value, replacement));
            }
        }
    }
}
