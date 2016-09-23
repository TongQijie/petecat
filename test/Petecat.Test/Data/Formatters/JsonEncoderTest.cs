using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Data.Formatters.Internal.Json;
using Petecat.Extension;

namespace Petecat.Test.Data.Formatters
{
    [TestClass]
    public class JsonEncoderTest
    {
        [TestMethod]
        public void GetPlainValue_1_Test()
        {
            var stringValue = "\n\b \\/hello\u4e2d";

            var byteValues = JsonEncoder.GetPlainValue(stringValue);

            Assert.IsTrue(JsonEncoder.GetString(byteValues) == "\"\\n\\b \\\\\\/hello中\"");
        }

        [TestMethod]
        public void GetPlainValue_2_Test()
        {
            var sourceString = "\\n\\b \\\\\\/hello中";

            var targetString = JsonEncoder.GetPlainValue(JsonEncoder.GetBytes(sourceString));

            Assert.IsTrue(targetString == "\n\b \\/hello\u4e2d");
        }
    }
}
