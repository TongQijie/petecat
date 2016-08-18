using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Data.Configuration;
using Petecat.Data.Access;
using Petecat.Extension;

using System.Linq;
using System;

namespace Petecat.Test.Data.Access
{
    [TestClass]
    public class DataCommandHelperTest
    {
        public DataCommandHelperTest()
        {
            DatabaseObjectManager.Instance.Initialize("./configuration/Databases.config".FullPath());
            DataCommandObjectManager.Instance.Initialize("./configuration/DataCommands.config".FullPath());
        }

        [TestMethod]
        public void GetDataCommand()
        {
            var dataCommandObject = DataCommandObjectManager.Instance.GetDataCommandObject("QuerySOInfo");
            dataCommandObject.SetParameterValues("@SONumber", 614393042, 614386922);
            var orders = dataCommandObject.QueryEntities<SO>();
        }

        [TestMethod]
        public void ExecuteTransaction()
        {
            var dataCommandObject = DataCommandObjectManager.Instance.GetDataCommandObject("InsertProduct");
            dataCommandObject.DatabaseObject.ExecuteTransaction((db) =>
            {
                var id = new Random().Next(100, 1000);
                
                dataCommandObject.SetParameterValue("@ID", id);
                dataCommandObject.SetParameterValue("@Name", id.ToString());
                dataCommandObject.SetParameterValue("@ListPrice", id);
                db.ExecuteNonQuery(dataCommandObject);

                id++;

                dataCommandObject = DataCommandObjectManager.Instance.GetDataCommandObject("InsertProduct", db);
                dataCommandObject.SetParameterValue("@ID", id);
                dataCommandObject.SetParameterValue("@Name", id.ToString());
                dataCommandObject.SetParameterValue("@ListPrice", id);
                db.ExecuteNonQuery(dataCommandObject);

                return true;
            });
        }
    }
}
