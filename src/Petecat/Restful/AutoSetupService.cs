using System;

namespace Petecat.Restful
{
    /// <summary>
    /// Class with this attribute will be auto register as a service. When MISLibraryContainer first initialize.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AutoSetupServiceAttribute : Attribute
    {
        /// <summary>
        /// Gets service type.
        /// </summary>
        public Type Service
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the sub key.
        /// </summary>
        /// <value>The sub key.</value>
        public string SubKey
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
        /// Gets or sets priority.
        /// </summary>
        public int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets constructor args. This constructor must be public.
        /// </summary>
        public object[] ConstructorArgs
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the AutoSetupServiceAttribute class.
        /// </summary>
        /// <param name="service">Service type.</param>
        public AutoSetupServiceAttribute(Type service)
        {
            this.Service = service;
            this.LifeTime = ServiceLifeTime.Singleton;
        }
    }
}
