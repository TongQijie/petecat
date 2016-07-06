using System;
namespace Petecat.Restful
{
    /// <summary>
    /// Service definition.
    /// </summary>
    internal class ServiceDefinition : IServiceDefinition
    {
        /// <summary>
        /// Gets or sets service type.
        /// </summary>
        public Type Service
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets implement type.
        /// </summary>
        public Type Implement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets life time.
        /// </summary>
        public ServiceLifeTime LifeTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets sub key.
        /// </summary>
        public string SubKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets service factory.
        /// </summary>
        public Func<object> ServiceFactory
        {
            get;
            set;
        }
    }
}
