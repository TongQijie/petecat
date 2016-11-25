using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Data.Access;
using Petecat.Extending;
using System.Data;
using System.Text.RegularExpressions;

namespace Petecat.Test.Data.Access
{
    [TestClass]
    public class DataCommandObjectTest
    {
        public DataCommandObjectTest()
        {
            DatabaseObjectManager.Instance.Initialize("./configuration/Databases.config".FullPath());
            DataCommandObjectManager.Instance.Initialize("./configuration/DataCommands.config".FullPath());
        }

        [TestMethod]
        public void ExecuteNonQueryTest()
        {
            var dataCommandObject = DataCommandObjectManager.Instance.GetDataCommandObject("RecordDeviceOperationData");
            dataCommandObject.SetParameterValue("@DeviceNumber", "123458");
            dataCommandObject.SetParameterValue("@CollectorId", "A04");
            dataCommandObject.SetParameterValue("@CharValue01", "9");
            dataCommandObject.SetParameterValue("@DateValue01", "2016-10-20 12:10:00");
            dataCommandObject.SetParameterValue("@StringValue01", "abc");
            dataCommandObject.SetParameterValue("@StringValue02", "def");
            dataCommandObject.SetParameterValue("@StringValue03", "ghu");
            dataCommandObject.SetParameterValue("@NumberValue01", 1.90);
            dataCommandObject.SetParameterValue("@NumberValue02", 222);
            dataCommandObject.SetParameterValue("@Remark", "123456");
            dataCommandObject.ExecuteNonQuery();
        }

        [TestMethod]
        public void FormatCommandTextTest()
        {
            var dataCommandObject = DataCommandObjectManager.Instance.GetDataCommandObject("QuerySOInfo_V1");
            dataCommandObject.FormatCommandText(0, "100152240", "100152260", "100152280");
            var orders = dataCommandObject.QueryEntities<SO>();
        }
    }
}
