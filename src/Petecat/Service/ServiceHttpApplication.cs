using Petecat.IOC;
using Petecat.Utility;
using Petecat.Logging;
using Petecat.Logging.Loggers;
using Petecat.Extension;

using System.Web;
using System;

namespace Petecat.Service
{
    public class ServiceHttpApplication : HttpApplication
    {
        public ServiceHttpApplication()
        {
            Initialize();
        }

        private void Initialize()
        {
            LoggerManager.SetLogger(new FileLogger(LoggerManager.AppDomainLoggerName, "./log".FullPath()));

            try
            {
                AppDomainContainer.Initialize(AppConfigUtility.GetAppConfig("containerAssemblies", string.Empty).FullPath());
                ServiceManager.Instance = new ServiceManager(AppDomainContainer.Instance);
            }
            catch (Exception e)
            {
                LoggerManager.GetLogger().LogEvent("ServiceHttpApplication", LoggerLevel.Fatal, e);
                return;
            }
        }
    }
}
