namespace Petecat.Restful
{
    /// <summary>
    /// Api framework services register interface.
    /// </summary>
    public interface IServicesRegister
    {
        /// <summary>
        /// Register services to host.
        /// </summary>
        /// <param name="host">App host.</param>
        void RegisterServicesTo(IAppHostBase host);
    }
}
