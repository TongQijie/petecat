using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Archiving;
using System.IO;

namespace Petecat.Test.Archiving
{
    [TestClass]
    public class ArchiverTest
    {
        [TestMethod]
        public void Archive()
        {
            var archiver = new Archiver("hey.arch", new string[] { "Petecat.ConsoleApp.exe", "Petecat.ConsoleApp.exe.config", "configuration" });

            archiver.Archive();
        }

        [TestMethod]
        public void Unarchive()
        {
            var archiver = new Archiver("hey", "hey.arch");
            archiver.Unarchive();
        }
    }
}
