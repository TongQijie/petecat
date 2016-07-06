using System.Collections.Generic;
namespace Petecat.Restful
{
    /// <summary>
    /// Services wiring strategy interface.
    /// </summary>
    public interface IServicesWiringStrategy
    {
        /// <summary>
        /// Gets priority.
        /// </summary>
        uint Priority
        {
            get;
        }

        /// <summary>
        /// Gets services definitions.
        /// </summary>
        /// <returns>Services definition.</returns>
        IEnumerable<IServiceDefinition> GetServicesDefinition();
    }
}
