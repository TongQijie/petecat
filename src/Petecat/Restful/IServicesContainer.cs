using System;
namespace Petecat.Restful
{
    /// <summary>
    /// Services container interface.
    /// </summary>
    public interface IServicesContainer : IServicesLocator, IServiceProvider
    {
        /// <summary>
        /// Create service scope.
        /// </summary>
        /// <returns>Service scope instance.</returns>
        IServicesScope CreateScope();
    }
}
