using System;
using System.Data.SqlClient;

namespace Petecat.Data.Repository
{
    public static class DataCommandHelper
    {
        public static IDataCommand GetDataCommand(Configuration.DatabaseInstance databaseInstance, Configuration.DataCommand dataCommand)
        {
            DatabaseObject databaseObject = null;
            if (string.IsNullOrEmpty(databaseInstance.Provider))
            {
                databaseObject = new DatabaseObject(SqlClientFactory.Instance, databaseInstance.ConnectionString);
            }
            else
            {
                throw new NotSupportedException();
            }

            var dataCommandObject = new DataCommandObject(databaseObject, dataCommand.CommandType, dataCommand.CommandText);
            if (dataCommand.Parameters != null && dataCommand.Parameters.Length > 0)
            {
                foreach (var parameter in dataCommand.Parameters)
                {
                    dataCommandObject.AddParameter(parameter.Name, parameter.DbType, parameter.Direction, parameter.Size);
                }
            }

            return dataCommandObject;
        }
    }
}
