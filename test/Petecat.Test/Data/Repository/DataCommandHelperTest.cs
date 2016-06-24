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
            DataCommandCache.Manager.Load("configuration/datacommands.config");
            DatabaseInstanceCache.Manager.Load("configuration/databases.config");

            var dataCommand = DataCommandHelper.GetDataCommand("eggsaver");
            //dataCommand.SetParameterValue("@CountryCode", "USA");
            dataCommand.SetParameterValues("@CountryCode", new string[] { "USA", "CAN" });
            var entities = dataCommand.QueryEntities<EggsaverModel>();
        }
    }
}
