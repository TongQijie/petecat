namespace Petecat.Restful
{
    /// <summary>
    /// Filter auto register.
    /// </summary>
    public interface IAutoRegisterFiltersRegister
    {
        /// <summary>
        /// Register auto register filters to host.
        /// </summary>
        /// <param name="host">App host.</param>
        void RegisterTo(IAppHostBase host);
    }
}
