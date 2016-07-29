using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Petecat.IOC
{
    public class AppDomainContainer : DefaultContainer
    {
        private static AppDomainContainer _Instance = null;

        public static AppDomainContainer Instance { get { return _Instance; } }

        public static AppDomainContainer Initialize()
        {
            if(_Instance == null)
            {
                _Instance = new AppDomainContainer();

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    _Instance.RegisterContainerAssembly(assembly);
                }

                var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

                foreach (var assembly in directory.GetFiles("*.dll", SearchOption.AllDirectories).Select(x => Assembly.LoadFile(x.FullName)))
                {
                    _Instance.RegisterContainerAssembly(assembly);
                }
            }

            return _Instance;
        }
    }
}
