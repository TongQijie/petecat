using System.IO;
using System.Linq;
using System.Reflection;

using Petecat.Extending;
using Petecat.DependencyInjection;

namespace Petecat.HttpServer.DependencyInjection
{
    public class HttpServerAssemblyContainer : AssemblyContainerBase
    {
        public HttpServerAssemblyContainer()
        {
            Directory = GetRootAssemblyDirectory(Assembly.GetExecutingAssembly().Location);

            foreach (var df in Directory.GetDirectories())
            {
                var i = df.GetDirectories().OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
                if (i != null)
                {
                    foreach (var fileInfo in i.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
                    {
                        Assemblies = Assemblies.Append(Assembly.LoadFile(fileInfo.FullName));
                    }
                }
            }
        }

        public DirectoryInfo Directory { get; private set; }

        private DirectoryInfo GetRootAssemblyDirectory(string currentAssemblyPath)
        {
            var directoryInfo = new FileInfo(currentAssemblyPath).Directory;
            // ASP.NET temperary folder.
            // iis: dl3
            // xsp: shadow
            while (directoryInfo.Name != "dl3" && directoryInfo.Name != "shadow")
            {
                directoryInfo = directoryInfo.Parent;
            }

            return directoryInfo;
        }
    }
}
