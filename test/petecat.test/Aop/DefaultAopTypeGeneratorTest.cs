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
            var proxyObject = AopProxyTypeFactory.GetProxyObject<AppleBase>(new BananaInterceptor());
            var result = proxyObject.SayHi("hi");
            Assert.IsTrue(result == "hi");

            result = proxyObject.SayTo("hello", "you");
            Assert.IsTrue(result == "you:hello");
        }
    }
}
