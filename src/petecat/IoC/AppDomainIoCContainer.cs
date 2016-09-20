using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Petecat.Logging;

namespace Petecat.IoC
{
    public class AppDomainIoCContainer : DefaultIoCContainer
    {
        private static AppDomainIoCContainer _Instance = null;

        public static AppDomainIoCContainer Instance { get { return _Instance; } }

        public static AppDomainIoCContainer Initialize()
        {
            if(_Instance == null)
            {
                _Instance = new AppDomainIoCContainer();

                try
                {
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        _Instance.RegisterContainerAssembly(assembly);
                    }

                    var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

                    foreach (var assembly in directory.GetFiles("*.dll", SearchOption.AllDirectories).Select(x => Assembly.LoadFile(x.FullName)))
                    {
                        _Instance.RegisterContainerAssembly(assembly);
                    }

                    foreach (var assembly in directory.GetFiles("*.exe", SearchOption.AllDirectories).Select(x => Assembly.LoadFile(x.FullName)))
                    {
                        _Instance.RegisterContainerAssembly(assembly);
                    }
                }
                catch (Exception e)
                {
                    LoggerManager.GetLogger().LogEvent("AppDomainIoCContainer", LoggerLevel.Warn, e);
                }
            }

            return _Instance;
        }
    }
}
