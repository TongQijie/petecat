using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Configuration;
using Petecat.Data.Configuration;
using Petecat.Data.Repository;

using System.Linq;

namespace Petecat.Test.Data.Repository
{
    [TestClass]
    public class DataCommandHelperTest
    {
        [TestMethod]
        public void GetDataCommand()
        {
            var configurationManager = new FileConfigurationManagerBase(true);
            configurationManager.LoadFromXml<DatabaseInstanceCollection>("databases.config", "Databases");
            configurationManager.LoadFromXml<DataCommandCollection>("dataCommands.config", "DataCommands");

            var databaseInstances = configurationManager.Get<DatabaseInstanceCollection>("Databases");
            var databaseInstance = databaseInstances.DatabaseInstances.FirstOrDefault(x => x.Key == "sql server");

            var dataCommands = configurationManager.Get<DataCommandCollection>("DataCommands");
            var dataCommand = dataCommands.DataCommands.FirstOrDefault(x => x.Key == "SQL-01");

            var dataCommandObject = DataCommandHelper.GetDataCommand(databaseInstance, dataCommand);
            var count = dataCommandObject.QueryScalar<int>();
        }
    }
}
