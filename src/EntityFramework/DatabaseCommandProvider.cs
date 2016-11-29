using Petecat.DependencyInjection.Attribute;

namespace Petecat.EntityFramework
{
    [DependencyInjectable(Inference = typeof(IDatabaseCommandProvider), Singleton = true)]
    public class DatabaseCommandProvider : IDatabaseCommandProvider
    {
        private IEntityFrameworkConfigurer _EntityFrameworkConfigurer;

        private IDatabaseProvider _DatabaseProvider;

        public DatabaseCommandProvider(IEntityFrameworkConfigurer entityFrameworkConfigurer,
            IDatabaseProvider databaseProvider)
        {
            _EntityFrameworkConfigurer = entityFrameworkConfigurer;
            _DatabaseProvider = databaseProvider;
        }

        public IDatabaseCommand GetDatabaseCommand(string name)
        {
            var databaseCommand = _EntityFrameworkConfigurer.GetDatabaseCommandItem(name);
            if (databaseCommand == null)
            {
                return null;
            }

            var database = _DatabaseProvider.Get(databaseCommand.Database);
            if (database == null)
            {
                return null;
            }

            return new DatabaseCommand(database, databaseCommand.CommandType, databaseCommand.CommandText);
        }
    }
}
