using System.Data.SqlClient;
using System.Reflection;

namespace Petecat.Data.Repository
{
    public static class DatabaseConfigManager
    {
        private static Configuration.DatabaseInstanceCluster _DatabaseInstanceCluster = null;

        public static Configuration.DatabaseInstanceCluster DatabaseInstanceCluster
        {
            get { return _DatabaseInstanceCluster ?? (_DatabaseInstanceCluster = new Configuration.DatabaseInstanceCluster()); }
        }

        private static Configuration.DataOperationCluster _DataOperationCluster = null;

        public static Configuration.DataOperationCluster DataOperationCluster
        {
            get { return _DataOperationCluster ?? (_DataOperationCluster = new Configuration.DataOperationCluster()); }
        }
        
        public static IDataCommand GetDataCommnad(string name)
        {
            if (_DataOperationCluster == null || _DatabaseInstanceCluster == null)
            {
                return null;
            }

            var dataOperation = _DataOperationCluster.DataOperations.Get(name, null);
            if (dataOperation == null)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("data operation {0} cannot be found.", name));
                return null;
            }

            var databaseInstance = _DatabaseInstanceCluster.DatabaseInstances.Get(dataOperation.Database, null);
            if (databaseInstance == null)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("database {0} cannot be found.", dataOperation.Database));
                return null;
            }

            var dataCommandObject = new DataCommandObject(new DatabaseObject(SqlClientFactory.Instance, databaseInstance.ConnectionString), dataOperation.CommandType, dataOperation.CommandText);
            if (dataOperation.Parameters != null && dataOperation.Parameters.Length > 0)
            {
                foreach (var parameter in dataOperation.Parameters)
                {
                    dataCommandObject.AddParameter(parameter.Name, parameter.DbType, parameter.Direction, parameter.Size);
                }
            }

            return dataCommandObject;
        }
    }
}