using Petecat.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace Petecat.Configuring.DependencyInjection
{
    class StaticFileConfigAssemblyContainer: AssemblyContainerBase
    {
        public StaticFileConfigAssemblyContainer()
        {
            RegisterAssemblies();
        }

        private void RegisterAssemblies()
        {
            var directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            foreach (var fileInfo in directoryInfo.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    RegisterAssembly(new StaticFileAssemblyInfo(Assembly.LoadFile(fileInfo.FullName)));
                }
                catch (Exception e)
                {
                    // TODO: throw
                }
            }

            foreach (var fileInfo in directoryInfo.GetFiles("*.exe", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    RegisterAssembly(new StaticFileAssemblyInfo(Assembly.LoadFile(fileInfo.FullName)));
                }
                catch (Exception e)
                {
                    // TODO: throw
                }
            }
        }
    }
}
