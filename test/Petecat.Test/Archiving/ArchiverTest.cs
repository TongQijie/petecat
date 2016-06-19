using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Archiving;

using System.IO;
using System.Linq;

namespace Petecat.Test.Archiving
{
    [TestClass]
    public class ArchiverTest
    {
        [TestMethod]
        public void Archive()
        {
            var directoryInfo = new DirectoryInfo(@"D:\Installer\Adobe Acrobat 9.0 Pro");

            var archiver = new Archiver(@"D:\Installer\Adobe_Acrobat_9.0_Pro.arch", directoryInfo.GetFiles().Select(x => x.FullName).ToArray());

            archiver.Archive();
        }

        [TestMethod]
        public void Unarchive()
        {
            var archiver = new Archiver(@"D:\Installer\Adobe_Acrobat_9.0_Pro", @"D:\Installer\Adobe_Acrobat_9.0_Pro.arch");
            archiver.Unarchive();
        }
    }
}
