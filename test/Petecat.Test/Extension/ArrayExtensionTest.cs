using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Extending;

namespace Petecat.Test.Extension
{
    [TestClass]
    public class ArrayExtensionTest
    {
        [TestMethod]
        public void SubArray_Test()
        {
            var source = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

            var buffer1 = source.Subset(2);

            var buffer2 = source.Subset(2, 10);
        }
    }
}
