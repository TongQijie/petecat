using System;

namespace Petecat.Configuration
{
    [Obsolete("replaced by CacheObjectManager")]
    public class RuntimeConfigurationManagerBase : AbstractConfigurationManager, IRuntimeConfigurationManager
    {
        public RuntimeConfigurationManagerBase()
            : base(false)
        {
        }
    }
}
