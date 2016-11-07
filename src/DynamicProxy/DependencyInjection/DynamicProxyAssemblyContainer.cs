using System;
using System.IO;
using System.Reflection;

using Petecat.DependencyInjection;

namespace Petecat.DynamicProxy.DependencyInjection
{
    public class DynamicProxyAssemblyContainer : AssemblyContainerBase
    {
        public DynamicProxyAssemblyContainer()
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
                    RegisterAssembly(new DynamicProxyAssemblyInfo(Assembly.LoadFile(fileInfo.FullName)));
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
                    RegisterAssembly(new DynamicProxyAssemblyInfo(Assembly.LoadFile(fileInfo.FullName)));
                }
                catch (Exception e)
                {
                    // TODO: throw
                }
            }
        }
    }
}
