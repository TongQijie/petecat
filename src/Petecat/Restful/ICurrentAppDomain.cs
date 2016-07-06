using System;
using System.Collections.Generic;
using System.Reflection;

namespace Petecat.Restful
{
    /// <summary>
    /// Current app domain interface.
    /// </summary>
    public interface ICurrentAppDomain
    {
        /// <summary>
        /// Gets base directory.
        /// </summary>
        string BaseDirectory
        {
            get;
        }

        /// <summary>
        /// Gets dynamic directory.
        /// </summary>
        string DynamicDirectory
        {
            get;
        }

        /// <summary>
        /// Get appdomain assemblies.
        /// </summary>
        /// <returns>Assembly collection.</returns>
        IEnumerable<Assembly> GetDomainAssemblies();

        /// <summary>
        /// Add assembly resolve handler.
        /// </summary>
        /// <param name="handler">Resolve event handler.</param>
        void AddAssemblyResolve(ResolveEventHandler handler);
    }
}
