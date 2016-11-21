using Petecat.Configuring;
using Petecat.DependencyInjection;
using Petecat.DependencyInjection.Attribute;
using Petecat.EntityFramework.Configuration;
using System;
using System.Linq;

namespace Petecat.EntityFramework
{
    [DependencyInjectable(Inference = typeof(IEntityFrameworkConfigurer), Singleton = true)]
    public class EntityFrameworkConfigurer : IEntityFrameworkConfigurer
    {
        private const string DatabaseCommandConfigurationCacheKey = "Global_DatabaseCommandConfiguration";

        public DatabaseCommandItemConfiguration GetDatabaseCommandItem(string name)
        {
            var configurations = DependencyInjector.GetObject<IStaticFileConfigurer>().GetValues<IDatabaseCommandConfiguration>(DatabaseCommandConfigurationCacheKey);
            if (configurations == null || configurations.Length == 0)
            {
                return null;
            }

            foreach (var configuration in configurations.Where(x => x.Commands != null && x.Commands.Length > 0))
            {
                foreach (var command in configuration.Commands)
                {
                    if (string.Equals(command.Name, name, StringComparison.OrdinalIgnoreCase))
                    {
                        return command;
                    }
                }
            }

            return null;
        }
    }
}
