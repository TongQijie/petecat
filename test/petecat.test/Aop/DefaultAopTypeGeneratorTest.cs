using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Aop;
using System;

namespace Petecat.Test.Aop
{
    [TestClass]
    public class DefaultAopTypeGeneratorTest
    {
        [TestMethod]
        public void Generate_Test()
        {
            //var type = new DefaultAopTypeGenerator().Generate(typeof(AppleBase));
            //var instance = Activator.CreateInstance(type, new BananaInterceptor()) as AppleBase;
            //instance.SayHi("hi");

            var instance = new AopAppleBase(new BananaInterceptor());
            Assert.IsTrue(instance.SayHi("aa") == "aa");
        }
    }
}
