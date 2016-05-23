using System;
using System.Collections.Generic;

namespace Petecat.Data.Repository
{
    public interface IDatabase
    {
        T QueryScalar<T>(IDataCommand dataCommand);

        List<T> QueryEntities<T>(IDataCommand dataCommand) where T : class, new();

        int ExecuteNonQuery(IDataCommand dbCommand);

        bool ExecuteTransaction(Func<IDatabase, bool> execution);
    }
}
