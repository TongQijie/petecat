using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Petecat.EntityFramework
{
    public interface IDatabase : IDisposable
    {
        DbProviderFactory DbProviderFactory { get; }

        T GetScalar<T>(IDatabaseCommand dataCommand);

        List<T> GetEntities<T>(IDatabaseCommand dataCommand) where T : class;

        int ExecuteNonQuery(IDatabaseCommand dbCommand);

        bool ExecuteTransaction(Func<IDatabase, bool> execution);
    }
}
