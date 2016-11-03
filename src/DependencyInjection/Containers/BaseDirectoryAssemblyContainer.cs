using Petecat.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace Petecat.DependencyInjection.Containers
{
    public class BaseDirectoryAssemblyContainer : AssemblyContainerBase
    {
        public BaseDirectoryAssemblyContainer()
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
                    RegisterAssembly(new AssemblyInfoBase(Assembly.LoadFile(fileInfo.FullName)));
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
                    RegisterAssembly(new AssemblyInfoBase(Assembly.LoadFile(fileInfo.FullName)));
                }
                catch (Exception e)
                {
                    // TODO: throw
                }
            }
        }
    }
}
