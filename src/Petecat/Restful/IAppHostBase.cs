using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
namespace Petecat.Restful
{
    /// <summary>
    /// App host base interface.
    /// </summary>
    public interface IAppHostBase
    {
        /// <summary>
        /// Gets rerquest filters.
        /// </summary>
        List<Action<HttpRequestWrapper, HttpResponseWrapper, object>> RequestFilters
        {
            get;
        }

        /// <summary>
        /// Initialize the application.
        /// </summary>
        void InitApp();

        /// <summary>
        /// Set ioc container.
        /// </summary>
        /// <param name="container">Ioc container.</param>
        void SetIOCContainer(IContainerAdapter container);

        /// <summary>
        /// Register services by assemblies.
        /// </summary>
        /// <param name="assemblies">Assemblies that contains services need to be registered.</param>
        void RegisterServices(params Assembly[] assemblies);

        /// <summary>
        /// Register validator by validator type.
        /// </summary>
        /// <param name="validator">Validator type.</param>
        void RegisterValidator(Type validator);
    }
}