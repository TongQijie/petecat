using Petecat.DependencyInjection;
using Petecat.HttpServer.DependencyInjection;
using Petecat.Logging;
using System;
using System.Web;
using Petecat.Extension;

namespace Petecat.HttpServer
{
    public class HttpApplicationBase : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                DependencyInjector.Setup(new RestServiceAssemblyContainer());
            }
            catch (Exception e)
            {
                DependencyInjector.GetObject<IFileLogger>().LogEvent("HttpApplicationBase", Severity.Fatal, e);
                return;
            }
        }
    }
}
