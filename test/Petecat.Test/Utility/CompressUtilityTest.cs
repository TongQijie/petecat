using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

using Petecat.Utility;

namespace Petecat.Test.Utility
{
    [TestClass]
    public class CompressUtilityTest
    {
        [TestMethod]
        public void Compress()
        {
            CompressUtility.GzipCompress(new FileInfo("Petecat.pdb"));
        }

        [TestMethod]
        public void Decompress()
        {
            CompressUtility.GzipDecompress(new FileInfo("Petecat.pdb.gz"));
        }

        [TestMethod]
        public void Archive()
        {
            CompressUtility.Archive(new FileInfo[] { new FileInfo("Petecat.Test.pdb"), new FileInfo("Petecat.Test.dll") }, "package.arch");
        }

        [TestMethod]
        public void Unarchive()
        {
            CompressUtility.Unarchive("package.arch", "package");
        }
    }
}
