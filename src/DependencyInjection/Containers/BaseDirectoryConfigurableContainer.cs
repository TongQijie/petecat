using Petecat.Extension;

namespace Petecat.DependencyInjection.Containers
{
    public class BaseDirectoryConfigurableContainer : ConfigurableContainerBase
    {
        public BaseDirectoryConfigurableContainer(params string[] paths)
        {
            if (paths != null && paths.Length > 0)
            {
                RegisterConfigurableFiles(paths);
            }
        }

        private void RegisterConfigurableFiles(string[] paths)
        {
            foreach (var path in paths)
            {
                var fullPath = path.FullPath();
                if (!fullPath.IsFile())
                {
                    // TODO: throw
                }

                RegisterConfigurableFile(new ConfigurableFileInfoBase(path));
            }
        }
    }
}