using System;
namespace Petecat.Restful
{
    /// <summary>
    /// Auto setup services container.
    /// </summary>
    public interface IAutoSetupServicesContainer : IServicesContainer, IServicesLocator, IServiceProvider
    {
        /// <summary>
        /// Gets priority for which implement will be used. The larger the number, the more priority. 
        /// </summary>
        int Priority
        {
            get;
        }
    }
}
