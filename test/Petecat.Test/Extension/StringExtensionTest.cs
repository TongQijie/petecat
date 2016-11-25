using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Extending;
using System;
using System.IO;

namespace Petecat.Test.Extension
{
    [TestClass]
    public class StringExtensionTest
    {
        [TestMethod]
        public void FullPath_1()
        {
            Assert.IsTrue(".".FullPath() == Path.Combine(AppDomain.CurrentDomain.BaseDirectory).Replace('\\', '/'));
            Assert.IsTrue("./".FullPath() == Path.Combine(AppDomain.CurrentDomain.BaseDirectory).Replace('\\', '/'));
            

        }
    }
}
