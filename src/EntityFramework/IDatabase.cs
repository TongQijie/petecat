using System;

namespace Petecat.EntityFramework
{
    public interface IDatabase : IDisposable
    {
        //DbProviderFactory DbProviderFactory { get; }

        //T QueryScalar<T>(IDataCommandObject dataCommand);

        //List<T> QueryEntities<T>(IDataCommandObject dataCommand) where T : class, new();

        //int ExecuteNonQuery(IDataCommandObject dbCommand);

        //bool ExecuteTransaction(Func<IDatabaseObject, bool> execution);
    }
}
