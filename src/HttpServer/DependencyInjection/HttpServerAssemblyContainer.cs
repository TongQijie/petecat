using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Petecat.Extending;
using Petecat.DependencyInjection;
using Petecat.Configuring.Attribute;
using Petecat.DependencyInjection.Attribute;
using Petecat.DynamicProxy.DependencyInjection;

namespace Petecat.HttpServer.DependencyInjection
{
    public class HttpServerAssemblyContainer : AssemblyContainerBase
    {
        public HttpServerAssemblyContainer()
        {
            Directory = GetRootAssemblyDirectory(Assembly.GetExecutingAssembly().Location);
        }

        public DirectoryInfo Directory { get; private set; }

        public void RegisterAssemblies<T>() where T : IAssemblyInfo
        {
            foreach (var df in Directory.GetDirectories())
            {
                var i = df.GetDirectories().OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
                if (i != null)
                {
                    foreach (var fileInfo in i.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
                    {
                        try
                        {
                            RegisterAssembly(typeof(T).CreateInstance<T>(Assembly.LoadFile(fileInfo.FullName)));
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
