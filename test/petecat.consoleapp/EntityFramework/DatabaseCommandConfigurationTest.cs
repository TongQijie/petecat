using Petecat.DependencyInjection;
using Petecat.EntityFramework;

namespace Petecat.ConsoleApp.EntityFramework
{
    public class DatabaseCommandConfigurationTest
    {
        public void Run()
        {
            var databaseCommandItem = DependencyInjector.GetObject<IEntityFrameworkConfigurer>().GetDatabaseCommandItem("test");
        }
    }
}