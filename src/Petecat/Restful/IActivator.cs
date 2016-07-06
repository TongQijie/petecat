using System;

namespace Petecat.Restful
{
    /// <summary>
    /// Create types of objects.
    /// </summary>
    public interface IActivator
    {
        /// <summary>
        /// Create instance with constructor injection.
        /// </summary>
        /// <param name="type">Requested type.</param>
        /// <returns>Type instance.</returns>
        object CreateInstanceWithConstructorInjection(Type type);
    }
}
