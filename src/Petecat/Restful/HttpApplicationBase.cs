using System.Web;

namespace Petecat.Restful
{
    public class HttpApplicationBase : HttpApplication
    {
        /// <summary>
        /// Services locator.
        /// </summary>
        protected IServicesLocator servicesLocator = null;

        /// <summary>
        /// Web application manager.
        /// </summary>
        protected IWebAppManager webAppManager = null;

        /// <summary>
        /// Initializes a new instance of the HttpApplication class.
        /// </summary>
        public HttpApplicationBase()
        {
            this.servicesLocator = ECLibraryContainer.Current;
            this.webAppManager = this.servicesLocator.Resolve<IWebAppManager>();
        }

        /// <summary>
        /// Application start handler.
        /// </summary>
        protected virtual void Application_Start()
        {
            if (this.webAppManager != null)
            {
                this.webAppManager.Start();
            }
        }

        /// <summary>
        /// Begin request handler.
        /// </summary>
        protected virtual void Application_BeginRequest()
        {
            if (this.webAppManager != null)
            {
                this.webAppManager.BeginRequest();
            }
        }

        /// <summary>
        /// End request handler.
        /// </summary>
        protected virtual void Application_EndRequest()
        {
            if (this.webAppManager != null)
            {
                this.webAppManager.EndRequest();
            }
        }

        /// <summary>
        /// Application error handler.
        /// </summary>
        protected virtual void Application_Error()
        {
        }
    }
}
