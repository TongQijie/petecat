namespace Petecat.Restful
{
    /// <summary>
    /// Validators register interface.
    /// </summary>
    public interface IValidatorsRegister
    {
        /// <summary>
        /// Register validators to host.
        /// </summary>
        /// <param name="host">App host.</param>
        void RegisterValidatorsTo(IAppHostBase host);
    }
}
