using Petecat.Configuration;

using System;
using System.Linq;

namespace Petecat.Data.Repository
{
    public class DatabaseInstanceCache
    {
        private static DatabaseInstanceCache _Manager = null;

        public static DatabaseInstanceCache Manager { get { return _Manager ?? (_Manager = new DatabaseInstanceCache()); } }

        public void Load(string databaseObjectConfigFile)
        {
            FileConfigurationManager = new FileConfigurationManagerBase(true);
            FileConfigurationManager.LoadFromXml<Configuration.DatabaseInstanceCollection>(databaseObjectConfigFile, "DatabaseObjects");
        }

        private FileConfigurationManagerBase FileConfigurationManager { get; set; }

        public Configuration.DatabaseInstance Get(string name)
        {
            if (FileConfigurationManager == null)
            {
                return null;
            }

            var databaseInstances = FileConfigurationManager.Get<Configuration.DatabaseInstanceCollection>("DatabaseObjects", null);
            if (databaseInstances == null || databaseInstances.DatabaseInstances == null || databaseInstances.DatabaseInstances.Length == 0)
            {
                return null;
            }

            return databaseInstances.DatabaseInstances.FirstOrDefault(x => x.Key.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}