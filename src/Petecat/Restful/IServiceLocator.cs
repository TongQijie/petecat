using System;
using System.Collections.Generic;

namespace Petecat.Restful
{
    /// <summary>
    /// The generic Service Locator interface. This interface is used to retrieve services (instances identified by type and optional name) from a container.
    /// </summary>
    public interface IServicesLocator : IServiceProvider
    {
        /// <summary>
        /// Check whether the service has been registered in service locator.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>True if the service has been registered; otherwise false.</returns>
        bool ContainService(Type serviceType);

        /// <summary>
        /// Check whether the service has been registered in service locator.
        /// </summary>
        /// <typeparam name="TService">Type of service.</typeparam>
        /// <returns>True if the service has been registered; otherwise false.</returns>
        bool ContainService<TService>();

        /// <summary>
        /// Check whether the service has been registered in service locator.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <param name="subKey">Sub key.</param>
        /// <returns>True if the service has been registered; otherwise false.</returns>
        bool ContainService(Type serviceType, string subKey);

        /// <summary>
        /// Check whether the service has been registered in service locator.
        /// </summary>
        /// <typeparam name="TService">Type of service.</typeparam>
        /// <param name="subKey">Sub key.</param>
        /// <returns>True if the service has been registered; otherwise false.</returns>
        bool ContainService<TService>(string subKey);

        /// <summary>
        /// Gets the service object collection of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object collection of type serviceType.-or- null if there is no service object of type serviceType.</returns>
        object GetAllServices(Type serviceType);

        /// <summary>
        /// Gets the service object of the specified type and key.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="subKey">Sub key.</param>
        /// <returns>A service object of type serviceType.-or- null if there is no service object of type serviceType.</returns>
        object GetService(Type serviceType, string subKey);

        /// <summary>
        /// Resolve Service Implementation.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <returns>Service Implementation object.</returns>
        TService Resolve<TService>();

        /// <summary>
        /// Resolve all Service Implementations.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <returns>Service Implementation object.</returns>
        IEnumerable<TService> ResolveAll<TService>();

        /// <summary>
        /// Resolve Service Implementation.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <param name="subKey">Sub key.</param>
        /// <returns>Service Implementation Instance.</returns>
        TService Resolve<TService>(string subKey);
    }
}
