using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Utility;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Petecat.Test.Utility
{
    [TestClass]
    public class ValueConverterTest
    {
        [TestMethod]
        public void Assignable()
        {
            var integer = Converter.Assignable<int>("3333");
            Assert.IsTrue(integer == 3333);
            var datetime = Converter.Assignable<DateTime>("2016-10-02 12:00:00");
            Assert.IsTrue(datetime == new DateTime(2016, 10, 2, 12, 0, 0));
        }

        public class AClass
        {
        }

        public interface AInterface
        {
        }

        public enum AEnum
        {
            A = 100,
        }
    }
}
