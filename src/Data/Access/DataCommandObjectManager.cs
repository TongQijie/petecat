using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Petecat.Caching;
using Petecat.Extension;
using Petecat.Data.Formatters;
using Petecat.Data.Configuration;

namespace Petecat.Data.Access
{
    public class DataCommandObjectManager
    {
        public const string CacheObjectName = "Global_DataCommandObjects";

        public const string CacheObjectNameFormat = "Global_DataCommandObjects_{0}";

        private List<string> CacheObjectNames = new List<string>();

        private static DataCommandObjectManager _Instance = null;

        public static DataCommandObjectManager Instance { get { return _Instance ?? (_Instance = new DataCommandObjectManager()); } }

        private DataCommandObjectManager() { }

        public void Initialize(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException(configPath);
            }

            CacheObjectManager.Instance.Add<DataCommandObjectSectionConfig>(string.Format(CacheObjectNameFormat, configPath),
                                                                            configPath,
                                                                            ObjectFormatterFactory.GetFormatter(ObjectFormatterType.Xml),
                                                                            true);

            CacheObjectNames.Add(string.Format(CacheObjectNameFormat, configPath));
        }

        public IDataCommandObject GetDataCommandObject(string name, IDatabaseObject databaseObject = null)
        {
            IDataCommandObject dataCommandObject = null;
            foreach (var cahceObjectName in CacheObjectNames)
            {
                var cacheObject = CacheObjectManager.Instance.GetObject(cahceObjectName);
                if (cacheObject == null)
                {
                    continue;
                }

                var dataCommandObjectSectionConfig = cacheObject.GetValue() as Configuration.DataCommandObjectSectionConfig;
                if (dataCommandObjectSectionConfig == null)
                {
                    continue;
                }

                var dataCommandObjectConfig = dataCommandObjectSectionConfig.DataCommands.FirstOrDefault(x => x.Key.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (dataCommandObjectConfig == null)
                {
                    continue;
                }

                if (databaseObject == null)
                {
                    databaseObject = DatabaseObjectManager.Instance.GetDatabaseObject(dataCommandObjectConfig.Database);
                }
                if (databaseObject == null)
                {
                    throw new Errors.DatabaseObjectNotFoundException(dataCommandObjectConfig.Database);
                }

                dataCommandObject = new DataCommandObject(databaseObject, dataCommandObjectConfig.CommandType, dataCommandObjectConfig.CommandText);
                if (dataCommandObjectConfig.Parameters != null && dataCommandObjectConfig.Parameters.Length > 0)
                {
                    foreach (var parameter in dataCommandObjectConfig.Parameters)
                    {
                        dataCommandObject.AddParameter(parameter.Name, parameter.DbType, parameter.Direction, parameter.Size);
                    }
                }

                break;
            }

            if (dataCommandObject == null)
            {
                throw new Errors.DataCommandObjectNotFoundException(name);
            }

            return dataCommandObject;
        }
    }
}
