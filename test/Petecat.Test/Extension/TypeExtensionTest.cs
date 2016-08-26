using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Extension.Attributes;
using Petecat.Extension;

namespace Petecat.Test.Extension
{
    [TestClass]
    public class TypeExtensionTest
    {
        public enum Method
        {
            [EnumValue("g")]
            Get,

            [EnumValue("p")]
            Post,
        }

        [TestMethod]
        public void GetEnumByValueTest()
        {
            var method = typeof(Method).GetEnumByValue("p");
            Assert.IsTrue(method.Equals(Method.Post));
            method = typeof(Method).GetEnumByValue("post");
            Assert.IsTrue(method.Equals(Method.Post));
        }

        [TestMethod]
        public void GetValueByEnumTest()
        {
            var method = typeof(Method).GetValueByEnum(Method.Post);
            Assert.IsTrue(method == "post");
        }
    }
}
