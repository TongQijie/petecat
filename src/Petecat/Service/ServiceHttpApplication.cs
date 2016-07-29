using Petecat.IoC;
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
        protected void Application_Start(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            LoggerManager.SetLogger(new FileLogger(LoggerManager.AppDomainLoggerName, "./log".FullPath()));

            try
            {
                AppDomainContainer.Initialize();
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
