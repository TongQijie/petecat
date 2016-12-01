using Petecat.Extending;
using Petecat.Configuring;
using Petecat.DependencyInjection.Attribute;
using Petecat.EntityFramework.Configuration;
using System.Linq;

namespace Petecat.EntityFramework
{
    [DependencyInjectable(Inference = typeof(IEntityFrameworkConfigurer), Singleton = true)]
    public class EntityFrameworkConfigurer : IEntityFrameworkConfigurer
    {
        private IStaticFileConfigurer _StaticFileConfigurer;

        public EntityFrameworkConfigurer(IStaticFileConfigurer staticFileConfigurer)
        {
            _StaticFileConfigurer = staticFileConfigurer;
        }

        public DatabaseCommandItemConfiguration GetDatabaseCommandItem(string name)
        {
            var configurations = _StaticFileConfigurer.GetValues<IDatabaseCommandConfiguration>();
            if (configurations == null || configurations.Length == 0)
            {
                return null;
            }

            foreach (var configuration in configurations.Where(x => x.Commands != null && x.Commands.Length > 0))
            {
                foreach (var command in configuration.Commands)
                {
                    if (command.Name.EqualsWith(name))
                    {
                        return command;
                    }
                }
            }

            return null;
        }

        public DatabaseItemConfiguration GetDatabaseItem(string name)
        {
            var configuration = _StaticFileConfigurer.GetValue<IDatabaseConfiguration>();
            if (configuration == null || configuration.Databases.Length == 0)
            {
                return null;
            }

            return configuration.Databases.FirstOrDefault(x => x.Name.EqualsWith(name));
        }
    }
}
