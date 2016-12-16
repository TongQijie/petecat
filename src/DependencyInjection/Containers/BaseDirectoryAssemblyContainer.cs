using System.IO;
using System.Reflection;

using Petecat.Extending;

namespace Petecat.DependencyInjection.Containers
{
    public class BaseDirectoryAssemblyContainer : AssemblyContainerBase
    {
        public BaseDirectoryAssemblyContainer()
        {
            var directoryInfo = new DirectoryInfo("./".FullPath());

            foreach (var fileInfo in directoryInfo.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
            {
                Assemblies = Assemblies.Append(Assembly.LoadFile(fileInfo.FullName));
            }

            foreach (var fileInfo in directoryInfo.GetFiles("*.exe", SearchOption.TopDirectoryOnly))
            {
                Assemblies = Assemblies.Append(Assembly.LoadFile(fileInfo.FullName));
            }
        }
    }
}
