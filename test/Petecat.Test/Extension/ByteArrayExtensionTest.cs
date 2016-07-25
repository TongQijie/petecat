using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Extension;

namespace Petecat.Test.Extension
{
    [TestClass]
    public class ByteArrayExtensionTest
    {
        [TestMethod]
        public void ToHexString()
        {
            var hexstring = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }.ToHexString();
        }
    }
}
