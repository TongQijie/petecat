using System;
using System.Collections.Generic;
using System.Reflection;
namespace Petecat.Restful
{
    /// <summary>
    /// Default app domain.
    /// </summary>
    [AutoSetupService(typeof(ICurrentAppDomain))]
    internal class CurrentAppDomain : ICurrentAppDomain
    {
        /// <summary>
        /// Current app domain.
        /// </summary>
        private AppDomain current = AppDomain.CurrentDomain;

        /// <summary>
        /// Gets base directory.
        /// </summary>
        public string BaseDirectory
        {
            get
            {
                return this.current.BaseDirectory;
            }
        }

        /// <summary>
        /// Gets dynamic directory.
        /// </summary>
        public string DynamicDirectory
        {
            get
            {
                return this.current.DynamicDirectory;
            }
        }

        /// <summary>
        /// Get appdomain assemblies.
        /// </summary>
        /// <returns>Assembly collection.</returns>
        public IEnumerable<Assembly> GetDomainAssemblies()
        {
            return this.current.GetAssemblies();
        }

        /// <summary>
        /// Add assembly resolve handler.
        /// </summary>
        /// <param name="handler">Resolve event handler.</param>
        public void AddAssemblyResolve(ResolveEventHandler handler)
        {
            this.current.AssemblyResolve += handler;
        }
    }
}
