namespace Petecat.DependencyInjection.Containers
{
    public class BaseDirectoryConfigurableContainer : ConfigurableContainerBase
    {
        public BaseDirectoryConfigurableContainer(string[] paths)
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
                RegisterConfigurableFile(new ConfigurableFileInfoBase(path));
            }
        }
    }
}