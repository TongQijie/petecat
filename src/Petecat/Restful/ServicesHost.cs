namespace Petecat.Restful
{
    /// <summary>
    /// Api framework services host.
    /// </summary>
    public class ServicesHost : HttpApplicationBase
    {
        /// <summary>
        /// App Start.
        /// </summary>
        protected override void Application_Start()
        {
            base.Application_Start();
            this.InitializeServicesHost();
        }

        /// <summary>
        /// Initializes services host.
        /// </summary>
        public virtual void InitializeServicesHost()
        {
            IAppHostBase hostbase = this.servicesLocator.Resolve<IAppHostBase>();
            hostbase.InitApp();
            hostbase.SetIOCContainer(this.servicesLocator.Resolve<IContainerAdapter>());
            this.servicesLocator.Resolve<IServicesRegister>().RegisterServicesTo(hostbase);
            this.servicesLocator.Resolve<IValidatorsRegister>().RegisterValidatorsTo(hostbase);
            this.servicesLocator.Resolve<IAutoRegisterFiltersRegister>().RegisterTo(hostbase);
        }
    }
}
