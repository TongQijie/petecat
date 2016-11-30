using Petecat.Extending;
using Petecat.DependencyInjection.Attribute;

using System;
using System.Reflection;
using System.Data.Common;
using System.Collections.Concurrent;

namespace Petecat.EntityFramework
{
    [DependencyInjectable(Inference = typeof(IDatabaseProvider), Singleton = true)]
    public class DatabaseProvider : IDatabaseProvider
    {
        private IEntityFrameworkConfigurer _EntityFrameworkConfigurer;

        public DatabaseProvider(IEntityFrameworkConfigurer entityFrameworkConfiguer)
        {
            _EntityFrameworkConfigurer = entityFrameworkConfiguer;
        }

        private ConcurrentDictionary<string, DbProviderFactory> CachedDbProviderFactories = new ConcurrentDictionary<string, DbProviderFactory>();

        public IDatabase Get(string name)
        {
            var database = _EntityFrameworkConfigurer.GetDatabaseItem(name);
            if (database == null)
            {
                return null;
            }

            if (!CachedDbProviderFactories.ContainsKey(database.Provider))
            {
                var factoryType = database.Provider.GetTypeByName();
                if (factoryType == null)
                {
                    return null;
                }

                var instance = factoryType.GetField("Instance", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
                if (instance == null)
                {
                    return null;
                }

                CachedDbProviderFactories.TryAdd(database.Provider, instance.GetValue(null) as DbProviderFactory);
            }

            DbProviderFactory factory;
            if (CachedDbProviderFactories.TryGetValue(database.Provider, out factory))
            {
                return new Database(factory, database.ConnectionString);
            }
            else
            {
                return null;
            }
        }
    }
}
