using System.Data;
using System.Data.Common;
using System.Collections.Generic;

namespace Petecat.EntityFramework
{
    public interface IDatabaseCommand
    {
        DbCommand GetDbCommand();

        IDatabase Database { get; }

        void AddParameter(string parameterName, DbType dbType, ParameterDirection direction, int size);

        void SetParameterValue(string parameterName, object parameterValue);

        void SetParameterValues(string parameterName, params object[] parameterValues);

        void FormatCommandText(int index, params object[] args);

        object GetParameterValue(string parameterName);

        T GetScalar<T>();

        List<T> GetEntities<T>() where T : class;

        int ExecuteNonQuery();
    }
}
