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
            var item = _EntityFrameworkConfigurer.GetDatabaseCommandItem(name);
            if (item == null)
            {
                return null;
            }

            var database = _DatabaseProvider.Get(item.Database);
            if (database == null)
            {
                return null;
            }

            var databaseCommand = new DatabaseCommand(database, item.CommandType, item.CommandText);
            if (item.Parameters != null && item.Parameters.Length > 0)
            {
                foreach (var parameter in item.Parameters)
                {
                    databaseCommand.AddParameter(parameter.Name, parameter.DbType, parameter.Direction, parameter.Size);
                }
            }

            return databaseCommand;
        }
    }
}
