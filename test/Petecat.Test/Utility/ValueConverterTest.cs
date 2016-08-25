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
        public void GetDefaultValue()
        {
            var defaultValue = Converter.GetDefaultValue(typeof(int));
            Assert.IsTrue((int)defaultValue == 0);
            defaultValue = Converter.GetDefaultValue(typeof(string));
            Assert.IsTrue((string)defaultValue == null);
            defaultValue = Converter.GetDefaultValue(typeof(AClass));
            Assert.IsTrue((AClass)defaultValue == null);
            defaultValue = Converter.GetDefaultValue(typeof(AEnum));
            Assert.IsTrue((int)((AEnum)defaultValue) == 0);
        }

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
