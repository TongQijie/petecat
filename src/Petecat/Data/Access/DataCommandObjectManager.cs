using System;
using System.IO;
using System.Linq;

using Petecat.Caching;
using System.Text;
using Petecat.Data.Formatters;

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

            CacheObjectManager.Instance.Add<Configuration.DataCommandObjectSectionConfig>(CacheObjectName, configPath, Encoding.UTF8,
                ObjectFormatterFactory.GetFormatter(ObjectFormatterType.Xml), true);
        }

        public IDataCommandObject GetDataCommandObject(string name)
        {
            var cacheObject = CacheObjectManager.Instance.GetObject(CacheObjectName);
            if (cacheObject == null)
            {
                throw new Errors.NotInitializedDataCommandObjectManagerException();
            }

            var dataCommandObjectSectionConfig = cacheObject.GetValue() as Configuration.DataCommandObjectSectionConfig;
            if (dataCommandObjectSectionConfig == null)
            {
                throw new Errors.DataCommandObjectNotFoundException(name);
            }

            var dataCommandObjectConfig = dataCommandObjectSectionConfig.DataCommands.FirstOrDefault(x => x.Key.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (dataCommandObjectConfig == null)
            {
                throw new Errors.DataCommandObjectNotFoundException(name);
            }

            var databaseObject = DatabaseObjectManager.Instance.GetDatabaseObject(dataCommandObjectConfig.Database);
            if (databaseObject == null)
            {
                throw new Errors.DatabaseObjectNotFoundException(dataCommandObjectConfig.Database);
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
