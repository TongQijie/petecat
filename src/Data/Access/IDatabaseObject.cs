using System;
using System.Data.Common;
using System.Collections.Generic;

namespace Petecat.Data.Access
{
    public interface IDatabaseObject : IDisposable
    {
        DbProviderFactory DbProviderFactory { get; }

        T QueryScalar<T>(IDataCommandObject dataCommand);

        List<T> QueryEntities<T>(IDataCommandObject dataCommand) where T : class, new();

        int ExecuteNonQuery(IDataCommandObject dbCommand);

        bool ExecuteTransaction(Func<IDatabaseObject, bool> execution);
    }
}
