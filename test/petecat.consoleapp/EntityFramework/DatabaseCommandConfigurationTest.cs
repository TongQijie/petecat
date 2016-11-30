using Petecat.DependencyInjection;
using Petecat.EntityFramework;

namespace Petecat.ConsoleApp.EntityFramework
{
    public class DatabaseCommandConfigurationTest
    {
        public void Run()
        {
            var command1 = DependencyInjector.GetObject<IDatabaseCommandProvider>().GetDatabaseCommand("test1");
            var apples1 = command1.GetEntities<AppleClass>();

            var command2 = DependencyInjector.GetObject<IDatabaseCommandProvider>().GetDatabaseCommand("test2");
            var bananas2 = command2.GetEntities<BananaClass>();

            var command3 = DependencyInjector.GetObject<IDatabaseCommandProvider>().GetDatabaseCommand("test3");
            command3.SetParameterValue("@ShoppingCartID", "2SHAJ8QZFNVYRDM");
            var apples3 = command3.GetEntities<AppleClass>();

            var command4 = DependencyInjector.GetObject<IDatabaseCommandProvider>().GetDatabaseCommand("test4");
            command4.SetParameterValue("@SONumber", 614393042);
            var bananas4 = command4.GetEntities<BananaClass>();

            var command5 = DependencyInjector.GetObject<IDatabaseCommandProvider>().GetDatabaseCommand("test5");
            command5.SetParameterValue("@SONumber", 614393042);
            var bananas5 = command5.GetEntities<CherryClass>();
        }
    }
}