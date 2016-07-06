using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petecat.Restful
{
    /// <summary>
    /// Service definition interface.
    /// </summary>
    public interface IServiceDefinition
    {
        /// <summary>
        /// Gets service type.
        /// </summary>
        Type Service
        {
            get;
        }

        /// <summary>
        /// Gets implement type.
        /// </summary>
        Type Implement
        {
            get;
        }

        /// <summary>
        /// Gets life time.
        /// </summary>
        ServiceLifeTime LifeTime
        {
            get;
        }

        /// <summary>
        /// Gets sub key.
        /// </summary>
        string SubKey
        {
            get;
        }

        /// <summary>
        /// Gets service factory.
        /// </summary>
        Func<object> ServiceFactory
        {
            get;
        }
    }
}
