using System;

namespace Petecat.Restful
{
    /// <summary>
    /// Services scope interface.
    /// </summary>
    public interface IServicesScope : IServicesLocator, IServiceProvider, IDisposable
    {
    }
}
