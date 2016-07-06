using System;
using System.Collections.Generic;
namespace Petecat.Restful
{
    /// <summary>
    /// Service definition container.
    /// </summary>
    public interface IServicesDefinitionContainer
    {
        /// <summary>
        /// Gets services definition.
        /// </summary>
        IEnumerable<IServiceDefinition> ServicesDefinition
        {
            get;
        }

        /// <summary>
        /// Register service with implement type and scope life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="implement">Implement type.</param>
        void RegisterService(Type service, Type implement);

        /// <summary>
        /// Register service with implement type and scope life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <typeparam name="TImplement">Implement type.</typeparam>
        void RegisterService<TService, TImplement>();

        /// <summary>
        /// Register service with implement type and scope life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="implement">Implement type.</param>
        /// <param name="subKey">Sub key.</param>
        void RegisterService(Type service, Type implement, string subKey);

        /// <summary>
        /// Register service with implement type and scope life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <typeparam name="TImplement">Implement type.</typeparam>
        /// <param name="subKey">Sub key.</param>
        void RegisterService<TService, TImplement>(string subKey);

        /// <summary>
        /// Register service with implement type and service life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="implement">Implement type.</param>
        /// <param name="lifeTime">Service life time.</param>
        void RegisterService(Type service, Type implement, ServiceLifeTime lifeTime);

        /// <summary>
        /// Register service with implement type and service life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <typeparam name="TImplement">Implement type.</typeparam>
        /// <param name="lifeTime">Service life time.</param>
        void RegisterService<TService, TImplement>(ServiceLifeTime lifeTime);

        /// <summary>
        /// Register service with implement type and service life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="implement">Implement type.</param>
        /// <param name="subKey">Sub key.</param>
        /// <param name="lifeTime">Service life time.</param>
        void RegisterService(Type service, Type implement, string subKey, ServiceLifeTime lifeTime);

        /// <summary>
        /// Register service with implement type and service life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <typeparam name="TImplement">Implement type.</typeparam>
        /// <param name="subKey">Sub key.</param>
        /// <param name="lifeTime">Service life time.</param>
        void RegisterService<TService, TImplement>(string subKey, ServiceLifeTime lifeTime);

        /// <summary>
        /// Register service with service factory and scope life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="serviceFactory">Service factory.</param>
        void RegisterService(Type service, Func<object> serviceFactory);

        /// <summary>
        /// Register service with service factory and scope life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <param name="serviceFactory">Service factory.</param>
        void RegisterService<TService>(Func<object> serviceFactory);

        /// <summary>
        /// Register service with service factory and scope life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="subKey">Sub key.</param>
        void RegisterService(Type service, Func<object> serviceFactory, string subKey);

        /// <summary>
        /// Register service with service factory and scope life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="subKey">Sub key.</param>
        void RegisterService<TService>(Func<object> serviceFactory, string subKey);

        /// <summary>
        /// Register service with service factory and service life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="lifeTime">Service life time.</param>
        void RegisterService(Type service, Func<object> serviceFactory, ServiceLifeTime lifeTime);

        /// <summary>
        /// Register service with service factory and service life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="lifeTime">Service life time.</param>
        void RegisterService<TService>(Func<object> serviceFactory, ServiceLifeTime lifeTime);

        /// <summary>
        /// Register service with service factory and service life time.
        /// </summary>
        /// <param name="service">Service type.</param>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="subKey">Sub key.</param>
        /// <param name="lifeTime">Service life time.</param>
        void RegisterService(Type service, Func<object> serviceFactory, string subKey, ServiceLifeTime lifeTime);

        /// <summary>
        /// Register service with service factory and service life time.
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <param name="serviceFactory">Service factory.</param>
        /// <param name="subKey">Sub key.</param>
        /// <param name="lifeTime">Service life time.</param>
        void RegisterService<TService>(Func<object> serviceFactory, string subKey, ServiceLifeTime lifeTime);
    }
}
