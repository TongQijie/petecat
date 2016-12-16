using System;
using System.Web;

using Petecat.Logging;
using Petecat.DependencyInjection;
using Petecat.HttpServer.DependencyInjection;
using Petecat.Configuring.DependencyInjection;
using Petecat.DynamicProxy.DependencyInjection;

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
                DependencyInjector.Setup(new HttpServerAssemblyContainer())
                    .RegisterAssemblies<AssemblyInfoBase>()
                    .RegisterAssemblies<StaticFileAssemblyInfo>()
                    .RegisterAssemblies<RestServiceAssemblyInfo>()
                    .RegisterAssemblies<WebSocketAssemblyInfo>()
                    .RegisterAssemblies<DynamicProxyAssemblyInfo>();
            }
            catch (Exception e)
            {
                DependencyInjector.GetObject<IFileLogger>().LogEvent("HttpApplicationBase", Severity.Fatal, e);
                return;
            }
        }
    }
}