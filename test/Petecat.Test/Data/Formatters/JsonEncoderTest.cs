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
    }
}
