using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Petecat.Logging;

namespace Petecat.IoC
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
                    try
                    {
                        _Instance.RegisterContainerAssembly(assembly);
                    }
                    catch (Exception e)
                    {
                        LoggerManager.GetLogger().LogEvent("AppDomainContainer", LoggerLevel.Warn, "failed to register assembly " + assembly.FullName, e);
                    }
                }
            }

            return _Instance;
        }
    }
}
