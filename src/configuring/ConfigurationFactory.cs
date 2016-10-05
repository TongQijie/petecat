namespace Petecat.Configuring
{
    public static class ConfigurationFactory
    {
        public static IConfigurationManager GetManager()
        {
            return ConfigurationManager.Instance;
        }
    }
}

