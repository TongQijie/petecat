using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Data.Repository;
using Petecat.Data.Entity;

using System;
using System.Text;

namespace Petecat.Test.Data.Repository
{
    [TestClass]
    public class DatabaseConfigManagerTest
    {
        [TestMethod]
        public void GetDataCommand()
        {
            Init();

            var dataCommand01 = DatabaseConfigManager.GetDataCommnad("SQL-01");
            var count = dataCommand01.QueryScalar<int>();
            Assert.AreEqual(count, 2649421);

            var dataCommand02 = DatabaseConfigManager.GetDataCommnad("SQL-02");
            dataCommand02.SetParameterValue("SiteId", 1);
            var warehouses = dataCommand02.QueryEntities<Warehouse>();
        }

        private void Init()
        {
            DatabaseConfigManager.DatabaseInstanceCluster.AddRange("configuration/databases.config", Encoding.UTF8);
            DatabaseConfigManager.DataOperationCluster.AddRange("configuration/operations.config", Encoding.UTF8);
        }

        public class Warehouse
        {
            [PlainDataMapping("ID")]
            public int Id { get; set; }

            [PlainDataMapping("CODE")]
            public string Code { get; set; }

            [PlainDataMapping("CRTT")]
            public DateTime CreateTime { get; set; }

            [PlainDataMapping("TYPE")]
            public int Type { get; set; }
        }
    }
}
