using Petecat.Extension;

using System;
using System.IO;
using System.Reflection;

namespace Petecat.DependencyInjection.Containers
{
    public class BaseDirectoryAssemblyContainer : AssemblyContainerBase
    {
        public BaseDirectoryAssemblyContainer()
        {
            RegisterAssemblies<AssemblyInfoBase>();
        }

        public void RegisterAssemblies<T>() where T : IAssemblyInfo
        {
            var directoryInfo = new DirectoryInfo("./".FullPath());

            foreach (var fileInfo in directoryInfo.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    RegisterAssembly(typeof(T).CreateInstance<T>(Assembly.LoadFile(fileInfo.FullName)));
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
                    RegisterAssembly(typeof(T).CreateInstance<T>(Assembly.LoadFile(fileInfo.FullName)));
                }
                catch (Exception e)
                {
                    // TODO: throw
                }
            }
        }
    }
}
