using System;
using System.IO;
using System.Linq;

using Petecat.Caching;

namespace Petecat.Data.Access
{
    public class DataCommandObjectManager
    {
        public const string CacheObjectName = "Global_DataCommandObjects";

        private static DataCommandObjectManager _Instance = null;

        public static DataCommandObjectManager Instance { get { return _Instance ?? (_Instance = new DataCommandObjectManager()); } }

        private DataCommandObjectManager() { }

        public void Initialize(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException(configPath);
            }

            CacheObjectManager.Instance.AddXml<Configuration.DataCommandObjectSectionConfig>(CacheObjectName, configPath, true);
        }

        public IDataCommandObject GetDataCommandObject(string name)
        {
            var cacheObject = CacheObjectManager.Instance.Get(CacheObjectName);
            if (cacheObject == null)
            {
                throw new Exception("data command object manager has not initialized.");
            }

            var dataCommandObjectSectionConfig = cacheObject.GetValue() as Configuration.DataCommandObjectSectionConfig;
            if (dataCommandObjectSectionConfig == null)
            {
                throw new Exception("data command object section not exists.");
            }

            var dataCommandObjectConfig = dataCommandObjectSectionConfig.DataCommands.FirstOrDefault(x => x.Key.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (dataCommandObjectConfig == null)
            {
                throw new Exception(string.Format("data command object {0} not exists.", name));
            }

            if (string.IsNullOrEmpty(dataCommandObjectConfig.Database))
            {
                throw new Exception("database field is empty.");
            }

            var databaseObject = DatabaseObjectManager.Instance.GetDatabaseObject(dataCommandObjectConfig.Database);
            if (databaseObject == null)
            {
                throw new Exception("database object not exists.");
            }

            var dataCommandObject = new DataCommandObject(databaseObject, dataCommandObjectConfig.CommandType, dataCommandObjectConfig.CommandText);
            if (dataCommandObjectConfig.Parameters != null && dataCommandObjectConfig.Parameters.Length > 0)
            {
                foreach (var parameter in dataCommandObjectConfig.Parameters)
                {
                    dataCommandObject.AddParameter(parameter.Name, parameter.DbType, parameter.Direction, parameter.Size);
                }
            }

            return dataCommandObject;
        }
    }
}
