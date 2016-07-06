using System;
using System.Collections.Generic;
using System.Reflection;
namespace Petecat.Restful
{
    /// <summary>
    /// Assembly type loader.
    /// </summary>
    public interface IAssemblyTypeLoader
    {
        /// <summary>
        /// Get all types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <returns>Type collection.</returns>
        IEnumerable<Type> GetTypes(Assembly assembly);

        /// <summary>
        /// Get all types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="message">Process message.</param>
        /// <returns>Type collection.</returns>
        IEnumerable<Type> GetTypes(Assembly assembly, out string message);

        /// <summary>
        /// Get filter types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="filter">Filter function.</param>
        /// <returns>Type collection.</returns>
        IEnumerable<Type> GetTypes(Assembly assembly, Func<Type, bool> filter);

        /// <summary>
        /// Get filter types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="filter">Filter function.</param>
        /// <param name="message">Process message.</param>
        /// <returns>Type collection.</returns>
        IEnumerable<Type> GetTypes(Assembly assembly, Func<Type, bool> filter, out string message);
    }
}
