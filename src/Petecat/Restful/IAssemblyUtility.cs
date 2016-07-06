using System;
using System.Reflection;

namespace Petecat.Restful
{
    /// <summary>
    /// Assembly utility interface.
    /// </summary>
    public interface IAssemblyUtility
    {
        /// <summary>
        /// Load reference assembly handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="args">Event args.</param>
        /// <returns>Loaded assembly.</returns>
        Assembly LoadReferenceAssemblyHandler(object sender, ResolveEventArgs args);
    }
}
