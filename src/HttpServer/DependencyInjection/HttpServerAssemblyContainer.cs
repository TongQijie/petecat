using Petecat.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Petecat.HttpServer.DependencyInjection
{
    public class HttpServerAssemblyContainer : AssemblyContainerBase
    {
        public HttpServerAssemblyContainer()
        {
            RegisterAssemblies();
        }

        private void RegisterAssemblies()
        {
            var directoryInfo = GetRootAssemblyDirectory(Assembly.GetExecutingAssembly().Location);

            foreach (var df in directoryInfo.GetDirectories())
            {
                var i = df.GetDirectories().OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
                if (i != null)
                {
                    foreach (var fileInfo in i.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
                    {
                        try
                        {
                            RegisterAssembly(new RestServiceAssemblyInfo(Assembly.LoadFile(fileInfo.FullName)));
                            RegisterAssembly(new WebSocketAssemblyInfo(Assembly.LoadFile(fileInfo.FullName)));
                            RegisterAssembly(new AssemblyInfoBase(Assembly.LoadFile(fileInfo.FullName)));
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

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
