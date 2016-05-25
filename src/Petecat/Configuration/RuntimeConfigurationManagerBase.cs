namespace Petecat.Configuration
{
    public class RuntimeConfigurationManagerBase : AbstractConfigurationManager, IRuntimeConfigurationManager
    {
        public RuntimeConfigurationManagerBase()
            : base(false)
        {
        }
    }
}
