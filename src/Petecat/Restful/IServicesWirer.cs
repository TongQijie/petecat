namespace Petecat.Restful
{
    /// <summary>
    /// Service wirer.
    /// </summary>
    public interface IServicesWirer
    {
        /// <summary>
        /// Gets services definition container.
        /// </summary>
        IServicesDefinitionContainer ServicesDefinitionContainer
        {
            get;
        }

        /// <summary>
        /// Wire the services.
        /// </summary>
        void Wire();
    }
}
