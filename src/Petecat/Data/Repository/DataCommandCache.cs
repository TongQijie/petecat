using Petecat.Configuration;

using System;
using System.Linq;

namespace Petecat.Data.Repository
{
    public class DataCommandCache
    {
        private static DataCommandCache _Manager = null;

        public static DataCommandCache Manager { get { return _Manager ?? (_Manager = new DataCommandCache()); } }

        public void Load(string dataCommandConfigFile)
        {
            FileConfigurationManager = new FileConfigurationManagerBase(true);
            FileConfigurationManager.LoadFromXml<Configuration.DataCommandCollection>(dataCommandConfigFile, "DataCommands");
        }

        private FileConfigurationManagerBase FileConfigurationManager { get; set; }

        public Configuration.DataCommand Get(string name)
        {
            if (FileConfigurationManager == null)
            {
                return null;
            }

            var dataCommands = FileConfigurationManager.Get<Configuration.DataCommandCollection>("DataCommands", null);
            if (dataCommands == null || dataCommands.DataCommands == null || dataCommands.DataCommands.Length == 0)
            {
                return null;
            }

            return dataCommands.DataCommands.FirstOrDefault(x => x.Key.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
