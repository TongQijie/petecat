using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text;

using Petecat.Data.Configuration;
using Petecat.Data.Xml;

namespace Petecat.Test.Data.Configuration
{
    [TestClass]
    public class DatabaseInstanceClusterTest
    {
        [TestMethod]
        public void Read()
        {
            var databaseCluster = DatabaseInstanceCluster.Read("configuration/databases.config", Encoding.UTF8);
            Assert.IsNotNull(databaseCluster);
        }

        [TestMethod]
        public void Write()
        {
            var databaseInstances = new DatabaseInstance[]
            {
                new DatabaseInstance() { Key = "mysql", ConnectionString = "this is a mysql connectionstring" },
                new DatabaseInstance() { Key = "sql server", ConnectionString = "this is a sql server connectionstring" },
                new DatabaseInstance() { Key = "oracle", ConnectionString = "this is a oracle connectionstring" },
                new DatabaseInstance() { Key = "sql server express", ConnectionString = "this is a sql server express connectionstring" },
            };
            var databaseInstanceCollection = new DatabaseInstanceCollection() { DatabaseInstances = databaseInstances };
            Serializer.WriteObject(databaseInstanceCollection, "configuration/databases.config", Encoding.UTF8);
        }
    }
}
