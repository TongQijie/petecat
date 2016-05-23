using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data.SqlClient;
using System.Data;

using Petecat.Data.Repository;

namespace Petecat.Test.Data.Repository
{
    [TestClass]
    public class DataCommandObjectTest
    {
        [TestMethod]
        public void Create()
        {
            var dataCommandObject = new DataCommandObject(SqlClientFactory.Instance, CommandType.Text, "select * from table where id = @id");
            dataCommandObject.SetParameterValue("@id", 1);
            dataCommandObject.SetParameterValue("@id", 3);
            dataCommandObject.SetParameterValue("@name", "name");
        }
    }
}
