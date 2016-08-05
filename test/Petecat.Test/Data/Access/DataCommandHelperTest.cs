using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Configuration;
using Petecat.Data.Configuration;
using Petecat.Data.Access;
using Petecat.Extension;

using System.Linq;

namespace Petecat.Test.Data.Access
{
    [TestClass]
    public class DataCommandHelperTest
    {
        [TestMethod]
        public void GetDataCommand()
        {
            DatabaseObjectManager.Instance.Initialize("./configuration/Databases.config".FullPath());
            DataCommandObjectManager.Instance.Initialize("./configuration/DataCommands.config".FullPath());

            var dataCommandObject = DataCommandObjectManager.Instance.GetDataCommandObject("QuerySOInfo");
            dataCommandObject.SetParameterValues("@SONumber", 614393042, 614386922);
            var orders = dataCommandObject.QueryEntities<SO>();
        }
    }
}
