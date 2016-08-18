using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Extension;

namespace Petecat.Test.Extension
{
    [TestClass]
    public class ArrayExtensionTest
    {
        [TestMethod]
        public void IndexOfEx_Test()
        {
            var source = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

            var index = source.IndexOfEx<byte>(new byte[] { 0x03, 0x04, 0x05 }, 0, source.Length);

            Assert.IsTrue(index == 2);

            int result;
            index = source.IndexOfEx<byte>(new byte[][] { new byte[] { 0x02, 0x03 }, new byte[] { 0x05 }}, 0, source.Length, out result);

            Assert.IsTrue(index == 1 && result == 0);
        }

        [TestMethod]
        public void SubArray_Test()
        {
            var source = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

            var buffer1 = source.SubArray(2);

            var buffer2 = source.SubArray(2, 10);
        }
    }
}
